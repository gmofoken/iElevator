using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    public class ElevatorUnit : INotifyPropertyChanged
    {
        private static readonly object lockObject = new object();
        private static ElevatorUnit instance = null;

        private int _currentFloor { get; set; }
        public int RequestedFloor { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public Direction IntendedDirection { get; private set; }
        private Queue<int> requests = new Queue<int>();
        public int id { get; private set; }
        public double weightLimit { get; set; }
        public int MaxCapacity { get; set; }
        private bool isIdle { get; set; } = true;
        private bool isRunning { get; set; } = true;
        public bool isDoorOpen { get; set; } = true;

        public bool IsDoorOpen
        {
            get { return isDoorOpen; }
            set 
            { 
                if (isDoorOpen != value)
                {
                    isDoorOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsIdle 
        {
            get { return isIdle; }
            set
            {
                if (value != isIdle)
                {
                    isIdle = value;
                    OnPropertyChanged();
                }
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ElevatorUnit(int initialFloor, int iD)
        {
            _currentFloor = initialFloor;
            CurrentDirection = Direction.None;
            id = iD;
            Task.Run(() => this.Run());
        }

        public int PlannedStops()
        {
            return requests.Count;
        }

        public int CurrentFLoor()
        {
            return this._currentFloor;
        }

        public bool IsActive()
        {
            return isRunning;
        }

        public void Run()
        {


            while (isRunning)
            {
                

                var test = id;

                ProcessRequests();

                Task.Delay(2000).Wait();
            }
        }

        private void Stop()
        {
            //if (!isIdle !)
        }


        private bool CheckWeight()
        {


            return false;
        }

        private void ProcessRequests()
        {
            if (requests.Count > 0) 
            {

                int nextFloor = requests.Peek();
                Console.WriteLine($"Elevator {id} Moving to floor {nextFloor}. CurrentFoor: {_currentFloor}");

                if (_currentFloor < nextFloor)
                    _currentFloor++;
                else if (_currentFloor > nextFloor)
                    _currentFloor--;

                //_currentFloor = (_currentFloor < nextFloor) ? _currentFloor++ : _currentFloor--;

                if (_currentFloor ==  nextFloor && requests.Count != 0)
                {
                    Console.WriteLine($"Elevator {id} Arrived at floor {nextFloor}. CurrentFoor: {_currentFloor}");
                    requests.Dequeue();
                }
                    


            }
            else
            {
                isIdle = true;
                CurrentDirection = Direction.None;
            }
        }

        public int CalculateDistance(int requestedFloor)
        {
            return Math.Abs(_currentFloor - requestedFloor);
        }

        public void QueueStops(int floor)
        {
            if (requests.Contains(floor))
            {
                Console.WriteLine($"Floor {floor} already Queued for elevator {id} currently at {_currentFloor}");
                
            }
            else
            {
                Console.WriteLine($"Floor {floor} Queued for elevator {id} at {_currentFloor}");
                requests.Enqueue(floor);
                IsIdle = false;
            }

            if (requests.Count > 0) 
                CurrentDirection = (_currentFloor < floor) ? Direction.Up : Direction.Down;
        }

        public bool WillPassRequestedFloor(ElevatorRequest request)
        {
            if (isIdle)
                return true;
            else if (CurrentDirection == Direction.Up && request.Floor > _currentFloor && request.Direction == CurrentDirection)
                return true;
            else if (CurrentDirection == Direction.Down && request.Floor < _currentFloor && request.Direction == CurrentDirection)
                return true;
            return false;
        }

    }
}
