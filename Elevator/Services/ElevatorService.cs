using Elevator.DTOs;
using Elevator.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator.Services
{
    public class ElevatorService
    {
        private static readonly Lazy<ElevatorService> elevatorService = new Lazy<ElevatorService>(() => new ElevatorService(), true);

        private List<ElevatorUnit> _elevators = new List<ElevatorUnit>();
        private int _numberOfFloors = 0;
        private Queue<ElevatorRequest> _awaitingQueues = new Queue<ElevatorRequest>();

        private ElevatorService() 
        {
            InitialiseElevators(2);
            Task.Run(() => ProcessQueues());
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

                Task.Delay(1000).Wait();
            }
        }

        public static ElevatorService Instance => elevatorService.Value;

        private void InitialiseElevators(int numOfElevators)
        {
            for (int i = 1; i <= numOfElevators; i++)
            {
                var elevator = new ElevatorUnit(new Random().Next(1, 10), i);
                
                _elevators.Add(elevator);
                _elevators.Last().PropertyChanged += (sender, e) =>
                {
                    var _l = sender as ElevatorUnit;

                    //_elevators.Where(x => x.id == _l.id).First().CallElevator();
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

        public Tuple<int, int> CallElevator(ElevatorRequest request)
        {
            var elevator = FindClosestElevator(request);

            if (elevator == null)
                _awaitingQueues.Enqueue(request);
            if (elevator != null && elevator.CurrentFLoor() != request.Floor) 
                elevator.QueueStops(request.Floor);

            return new Tuple<int, int>(elevator.id, elevator.CurrentFLoor());
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
