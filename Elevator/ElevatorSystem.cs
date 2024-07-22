using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Users;

namespace Elevator
{
    public class ElevatorUnit : Elevators
    {
        
        private Queue<int> requests = new Queue<int>();
        public int ID { get; private set; }
        private bool isIdle { get; set; } = true;
        private bool isRunning { get; set; } = true;
        private bool IsDoorOpen { get; set; } = false;
        private State state { get; set; }

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

        public State State
        {
            get { return state; }
            set
            {
                if (value != state)
                {
                    state = value;
                    OnPropertyChanged();
                }
            }
        }

        public ElevatorUnit(int initialFloor, int iD, ElevatorTypeEnum type) : base(type)
        {
            CurrentFloor = initialFloor;
            CurrentDirection = Direction.None;
            ID = iD;
            Task.Run(() => this.Run());
        }

        public int PlannedStops()
        {
            return requests.Count;
        }

        public int CurrentFLoor()
        {
            return this.CurrentFloor;
        }

        public bool IsActive()
        {
            return isRunning;
        }

        public void Run()
        {


            while (isRunning)
            {
                if (requests.Count == 0)
                {
                    isIdle = true;
                    state = State.Waiting;
                }
                if (requests.Count() > 0 && state != State.Loading)
                {
                    state = State.Moving;

                    ProcessRequests();

                    Task.Delay(Speed).Wait();
                }
                
            }
        }

        public State GetCurrentState()
        {
            return state;
        }

        public Direction GetCurrentDirection() 
        {
            if (requests.Count == 0)
                return Direction.None;
            else
                return ((CurrentFLoor() - requests.Peek()) > 0) ? Direction.Up : Direction.Down;
        }

        private void Stop()
        {
            while (state == State.Loading)
            {
            }
        }



        public void AcitvateDoors() 
        { 
            if (IsDoorOpen)
            {
                IsDoorOpen = false;
                State = State.Moving;
                Console.WriteLine(string.Format("Elevator {0} door is >>closing<<", ID));
            }
            else
            {
                IsDoorOpen = true;
                Console.WriteLine(string.Format("Elevator {0} door is <<opening>>", ID));
                for (int i = 0; i < Load.Users.Count; i++)
                {
                    if (Load.Users[i] != null && Load.Users[i].DestinationFloor == CurrentFloor)
                    {
                        Console.WriteLine($"{Load.Users[i].Id} has disembarked. ");
                        Load.Weight -= Load.Users[i].Weight;
                        Load.Capacity--;
                        Load.Users.RemoveAt(i);

                    }
                }
            }

        }

        public StateResponse LoadUsers(User user)
        {
            if (this.Load.Capacity >= this.MaxCapacity)
            {
                Console.WriteLine($"Elevator {ID}: Capacity limit reached.");
                return StateResponse.WeightException;
            }
            else if (!CheckWeight(user.Weight))
            {
                Console.WriteLine($"Elevator {ID}: Weight limit reached.");
                return StateResponse.WeightException;
            }
            

            this.Load.Users.Add(user);

            Load.Weight += user.Weight;
            Load.Capacity++;

            Console.WriteLine($"User {user.Id} boarded the elevator.");

            QueueStops(user.DestinationFloor);
            return StateResponse.Success;
        }

        private bool CheckWeight(double weight)
        {
            if ((this.Load.Weight + weight) > this.MaxWeight)
                return false;
            return true;
        }


        private void ProcessRequests()
        {
            if (requests.Count > 0) 
            {

                int nextFloor = requests.Peek();
                Console.WriteLine($"Elevator {ID} Moving to floor {nextFloor}. CurrentFoor: {CurrentFloor}");

                if (CurrentFloor < nextFloor)
                    CurrentFloor++;
                else if (CurrentFloor > nextFloor)
                    CurrentFloor--;


                if (CurrentFloor ==  nextFloor && requests.Count != 0)
                {
                    Console.WriteLine($"Elevator {ID} Arrived at floor {nextFloor}. CurrentFoor: {CurrentFloor}");
                    state = State.Loading;
                    requests.Dequeue();
                    Stop();
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
            return Math.Abs(CurrentFloor - requestedFloor);
        }

        public void QueueStops(int floor)
        {
            if (requests.Contains(floor))
            {
                Console.WriteLine($"Floor {floor} already Queued for elevator {ID} currently at {CurrentFloor}");                
            }
            else
            {
                Console.WriteLine($"Floor {floor} Queued for elevator {ID} at {CurrentFloor}");
                requests.Enqueue(floor);
                IsIdle = false;
            }

            if (requests.Count > 0) 
                CurrentDirection = (CurrentFloor < floor) ? Direction.Up : Direction.Down;
        }

        public bool WillPassRequestedFloor(ElevatorRequest request)
        {
            if (isIdle)
                return true;
            else if (CurrentDirection == Direction.Up && request.Floor > CurrentFloor && request.Direction == CurrentDirection)
                return true;
            else if (CurrentDirection == Direction.Down && request.Floor < CurrentFloor && request.Direction == CurrentDirection)
                return true;
            return false;
        }

    }
}
