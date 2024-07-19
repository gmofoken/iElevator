using Elevator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class ElevatorUnit
    {
        public int CurrentFloor { get; private set; }
        public Direction CurrentDirection { get; private set; }
        private Queue<int> requests = new Queue<int>();
        public int id { get; private set; }

        public ElevatorUnit(int initialFloor, int iD)
        {
            CurrentFloor = initialFloor;
            CurrentDirection = Direction.None;
            id = iD;
        }

        public void HandleRequest(int requestedFloor)
        {
            Direction requestedDirection = requestedFloor > CurrentFloor ? Direction.Up : Direction.Down;
            requests.Enqueue(requestedFloor);

            if (CurrentDirection == Direction.None)
                CurrentDirection = requestedDirection;
            else if (requestedDirection != CurrentDirection)
                CurrentDirection = requestedDirection;
        }

        public void ProcessRequests()
        {
            while (requests.Count > 0)
            {
                int nextFloor = requests.Dequeue();
                Console.WriteLine($"Moving to floor {nextFloor}");
                CurrentFloor = 1;
                // Other elevator operations (e.g., open doors, close doors, etc.)
            }
        }

        public int CalculateDistance(int requestedFloor)
        {
            return Math.Abs(CurrentFloor - requestedFloor);
        }

        public bool WillPassRequestedFloor(int requestedFloor)
        {
            if (CurrentDirection == Direction.Up && requestedFloor > CurrentFloor)
                return true;
            else if (CurrentDirection == Direction.Down && requestedFloor < CurrentFloor)
                return true;
            return false;
        }

        public bool IsStationary()
        {
            return CurrentDirection == Direction.None;
        }
    }
}
