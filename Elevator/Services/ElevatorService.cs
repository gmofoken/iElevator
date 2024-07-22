using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Models;
using Shared.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Users;
using Users.Enums;

namespace Elevator.Services
{
    public class ElevatorService
    {
        private static readonly Lazy<ElevatorService> elevatorService;

        private List<ElevatorUnit> _elevators = new List<ElevatorUnit>();
        private int _numberOfFloors = 0;
        private Queue<ElevatorRequest> _awaitingQueues = new Queue<ElevatorRequest>();
        private List<Floor> _floors = new List<Floor>();

        private bool _isElevatorLoading = false;

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ElevatorService(List<ElevatorTypeEnum> elevators, int floors)
        {
            InitialiseElevators(elevators);
            SetNumberOfFloors(floors);
            InitialiseFloorQueues();
            Task.Run(() => ProcessQueues());
        }


        public static ElevatorService Instance => elevatorService.Value;

        public void InitialiseFloorQueues()
        {
            for (int i = 1; i <= 20; i++)
                _floors.Add(new Floor(i));
        }

        private void ProcessQueues()
        {
            while (true)
            {
                if (_awaitingQueues.Count > 0)
                {
                    var request = _awaitingQueues.Dequeue();

                    var elevator = FindClosestElevator(request);

                    if (elevator != null)
                        elevator.QueueStops(request.Floor);
                    else
                        _awaitingQueues.Enqueue(request);
                }

                _elevators.Where(x => x.State == State.Loading).ToList().ForEach(elevator => {
                    if (elevator.State == State.Loading)
                    {
                        _isElevatorLoading = true;

                        elevator.AcitvateDoors();

                        var floor = _floors.Where(x => x.FloorID == elevator.CurrentFLoor()).First();

                        Queue<User> queue = null;
                        Queue<User> newQueue = new Queue<User>();

                        var currentDirection = elevator.GetCurrentDirection();



                        if (currentDirection == Direction.None)
                            queue = (floor.Queues.Up.Count > 0) ? floor.Queues.Up : floor.Queues.Down;
                        else if (currentDirection == Direction.Up)
                            queue = floor.Queues.Up;
                        else
                            queue = floor.Queues.Down;

                        while (queue.Count > 0)
                        {
                            var user = queue.Dequeue();

                            var load = elevator.LoadUsers(user);

                            if (load != StateResponse.Success)
                                newQueue.Enqueue(user);
                        }

                        if (elevator.CurrentDirection == Direction.Up)
                        {
                            _floors.Where(x => x.FloorID == elevator.CurrentFLoor()).First().Queues.Up = newQueue;
                        }
                        else
                            _floors.Where(x => x.FloorID == elevator.CurrentFLoor()).First().Queues.Up = newQueue;


                        Task.Delay(2000).Wait();


                        _isElevatorLoading = false;
                        elevator.AcitvateDoors();
                    }

                });
            }
        }

        private void LoadUsers()
        {

        }


        private void InitialiseElevators(List<ElevatorTypeEnum> elevetors)
        {
            for (int i = 1; i <= elevetors.Count; i++)
            {
                var elevator = new ElevatorUnit(new Random().Next(1, 10), i, elevetors[i-1]);

                var used = false;

                _elevators.Add(elevator);
                _elevators.Last().PropertyChanged += (sender, e) =>
                {
                    var _l = sender as ElevatorUnit;

                    var state = _l.State;

                    if (state == State.Loading)
                    {

                    }
                };

            }
        }

        public List<ElevatorUnit> ActiveElevators()
        {
            return _elevators.Where(x => x.IsActive()).ToList();
        }

        public void SetNumberOfFloors(int floors)
        {
            _numberOfFloors = floors;
        }

        public void CallElevator(ElevatorRequest request)
        {


            try
            {
                var elevator = FindClosestElevator(request);

                elevator.QueueStops(request.Floor);
            }
            catch (Exception)
            {
                _awaitingQueues.Enqueue(request);
            }



        }

        public void AddUserToQueue(User user) 
        {
            try
            {
                if (user.CurrentFloor - user.DestinationFloor > 0)
                {
                    _floors.Where(x => x.FloorID == user.CurrentFloor).FirstOrDefault().Queues.Down.Enqueue(user);
                    _floors.Where(x => x.FloorID == user.CurrentFloor).FirstOrDefault().IsThereGroupGoingDown = true;
                }
                else
                {
                    _floors.Where(x => x.FloorID == user.CurrentFloor).FirstOrDefault().Queues.Up.Enqueue(user);
                    _floors.Where(x => x.FloorID == user.CurrentFloor).FirstOrDefault().IsThereGroupGoingUp = true;
                }
            }
            catch
            {
                Console.WriteLine("Invalid floor selection");
            }
        }

        private ElevatorUnit FindClosestElevator(ElevatorRequest request)
        {
            ElevatorUnit closestElevator = null;
            int minDistance = _numberOfFloors;

            foreach (var elevator in _elevators)
            {
                int distance = elevator.CalculateDistance(request.Floor);
                if (distance < minDistance)
                {
                    if (elevator.WillPassRequestedFloor(request)) 
                    {
                        closestElevator = elevator;
                        minDistance = distance;
                    }
                        
                }
            }

            return closestElevator;
        }
    }
}
