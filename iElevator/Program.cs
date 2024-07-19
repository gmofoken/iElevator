using ElevatorControlSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using Elevator;
using ElevatorControl;

namespace iElevator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var elevatorControl = new ElevatorControlUnit(2);


            while (true)
            {


                Random rnd = new Random();
                int requestedFloor = rnd.Next(1, 10);

                var closestElevator = elevatorControl.FindClosestElevator(elevatorControl.GetElevators(), requestedFloor);


                if (closestElevator != null)
                {
                    Console.WriteLine($"Sending elevator {closestElevator.id} to floor {requestedFloor}");
                    closestElevator.ProcessRequests();
                }
                else
                {
                    Console.WriteLine("No suitable elevator found.");
                }

                Thread.Sleep(1000);
            }



            //var elevatorControl = new ElevatorControl();
            //var elevator1Display = new ElevatorDisplay();
            //var elevator2Display = new ElevatorDisplay();

            //// Register observers for different elevators
            //elevatorControl.RegisterObserver(1, elevator1Display);
            //elevatorControl.RegisterObserver(2, elevator2Display);

            //// Simulate button presses
            //elevatorControl.ButtonPressed(1, 3);
            //elevatorControl.ButtonPressed(2, 5);

            //// Unregister observer for elevator 1
            ////elevatorControl.UnregisterObserver(1, elevator1Display);

            //// No notification for elevator 1 after unregistering
            //elevatorControl.ButtonPressed(1, 4);

        }
    }
}
