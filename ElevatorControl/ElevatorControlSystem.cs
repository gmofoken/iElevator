using System;
using System.Collections.Generic;

namespace ElevatorControlSystem
{
    public interface IElevatorControl
    {
        void RegisterObserver(int elevatorId, IButtonPressObserver observer);

        void UnregisterObserver(int elevatorId, IButtonPressObserver observer);

        void ButtonPressed(int elevatorId, int floor);
    }

    public class ElevatorControl : IElevatorControl
    {
        private Dictionary<int, List<IButtonPressObserver>> elevatorObservers = new Dictionary<int, List<IButtonPressObserver>>();

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
    }

    public interface IButtonPressObserver
    {
        void HandleButtonPress(int floor);
    }

    public class ElevatorDisplay : IButtonPressObserver
    {
        public void HandleButtonPress(int floor)
        {
            Console.WriteLine($"Elevator Display: Going to floor {floor}");
        }
    }
}
