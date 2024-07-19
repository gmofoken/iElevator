using Elevator;
using ElevatorControl.Interfaces;
using ElevatorControlSystem.Interfaces;
using System;
using System.Collections.Generic;

namespace ElevatorControl
{
    

    public class ElevatorControlUnit : IElevatorControlUnit
    {
        private Dictionary<int, List<IButtonPressObserver>> elevatorObservers = new Dictionary<int, List<IButtonPressObserver>>();
        private List<ElevatorUnit> elevators = new List<ElevatorUnit>();
        public int MyProperty { get; set; }

        public ElevatorControlUnit(int numberOfElevators) 
        {
            InitializeElevators(numberOfElevators);
        }

        private void InitializeElevators(int numOfElevators)
        {
            for (int i = 1; i <= numOfElevators; i++) 
            {
                elevators.Add(new ElevatorUnit(new Random().Next(1, 10), i));
            }
        }

        public List<ElevatorUnit> GetElevators()
        {
            return elevators;
        }

        public void RegisterObserver(int elevatorId, IButtonPressObserver observer)
        {
            if (!elevatorObservers.ContainsKey(elevatorId))
                elevatorObservers[elevatorId] = new List<IButtonPressObserver>();

            elevatorObservers[elevatorId].Add(observer);
        }

        public void UnregisterObserver(int elevatorId, IButtonPressObserver observer)
        {
            if (elevatorObservers.ContainsKey(elevatorId))
                elevatorObservers[elevatorId].Remove(observer);
        }

        public void ButtonPressed(int elevatorId, int floor)
        {
            Console.WriteLine($"Elevator {elevatorId} called to floor {floor}");
            NotifyObservers(elevatorId, floor);
        }

        private void NotifyObservers(int elevatorId, int floor)
        {
            if (elevatorObservers.ContainsKey(elevatorId))
            {
                foreach (var observer in elevatorObservers[elevatorId])
                {
                    observer.HandleButtonPress(floor);
                }
            }
        }


        public ElevatorUnit FindClosestElevator(List<ElevatorUnit> elevators, int requestedFloor)
        {
            ElevatorUnit closestElevator = null;
            int minDistance = 10;

            foreach (var elevator in elevators)
            {
                int distance = elevator.CalculateDistance(requestedFloor);
                if (distance < minDistance)
                {
                    closestElevator = elevator;
                    minDistance = distance;
                }
                else if (distance == minDistance)
                {
                    if (elevator.WillPassRequestedFloor(requestedFloor))
                        closestElevator = elevator;
                }
            }

            return closestElevator;
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
