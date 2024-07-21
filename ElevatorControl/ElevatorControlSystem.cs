using Elevator;
using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Services;
using ElevatorControl.DTOs;
using ElevatorControl.Interfaces;
using ElevatorControlSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Users;
using Users.Enums;

namespace ElevatorControl
{
    

    public class ElevatorControlUnit :  IElevatorControlUnit
    {
        private Dictionary<int, List<IButtonPressObserver>> _elevatorObservers = new Dictionary<int, List<IButtonPressObserver>>();
        private int _numberOfFloors = 0;
        private List<Floor> _floors = new List<Floor>();

        private readonly ElevatorService _elevatorService = ElevatorService.Instance;

        public ElevatorControlUnit(int numberOfElevators, int numberOfFloors)
        {
            _numberOfFloors = numberOfFloors;
            InitialiseFloorQueues();

            _elevatorService.SetNumberOfFloors(20);
        }

        public void QueueUsers(int currentFloor, int targetFloor, double weight)
        {
            var user = new User()
            {
                CurrentFloor = currentFloor,
                DestinationFloor = targetFloor,
                Weight = weight,
                CurrentAction = UserAction.Waiting
            };

            if (currentFloor - targetFloor > 0)
            {
                user.direction = Direction.Down;
                _floors.Where(x => x.FloorID == currentFloor).FirstOrDefault().Queues.Down.Enqueue(user);
                _floors.Where(x => x.FloorID == currentFloor).FirstOrDefault().IsThereGroupGoingDown = true;
            }
            else
            {
                user.direction = Direction.Up;
                _floors.Where(x => x.FloorID == currentFloor).FirstOrDefault().Queues.Up.Enqueue(user);
                _floors.Where(x => x.FloorID == currentFloor).FirstOrDefault().IsThereGroupGoingUp = true;
            }

            var request = new ElevatorRequest()
            {
                Floor = currentFloor,
                Direction = user.direction
            };

            _elevatorService.CallElevator(request);
        }


        private void InitialiseFloorQueues()
        {
            for (int i = 1; i <= _numberOfFloors; i++)
                _floors.Add(new Floor(i));
        }

        public void RegisterObserver(int elevatorId, IButtonPressObserver observer)
        {
            if (!_elevatorObservers.ContainsKey(elevatorId))
                _elevatorObservers[elevatorId] = new List<IButtonPressObserver>();

            _elevatorObservers[elevatorId].Add(observer);
        }

        public void UnregisterObserver(int elevatorId, IButtonPressObserver observer)
        {
            if (_elevatorObservers.ContainsKey(elevatorId))
                _elevatorObservers[elevatorId].Remove(observer);
        }

        public void ButtonPressed(int elevatorId, int floor)
        {
            Console.WriteLine($"Elevator {elevatorId} called to floor {floor}");
            NotifyObservers(elevatorId, floor);
        }

        private void NotifyObservers(int elevatorId, int floor)
        {
            if (_elevatorObservers.ContainsKey(elevatorId))
            {
                foreach (var observer in _elevatorObservers[elevatorId])
                {
                    observer.HandleButtonPress(floor);
                }
            }
        }
    }

    

    public class ElevatorDisplay : IButtonPressObserver
    {
        public void HandleButtonPress(int floor)
        {
            Console.WriteLine($"Elevator Display: Going to floor {floor}");
        }
    }

    
}
