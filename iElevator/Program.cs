using ElevatorControlSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using Elevator;
using ElevatorControl;
using System.IO;

namespace iElevator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var elevatorControl = new ElevatorControlUnit(2, 20);

            string file = @"Users.txt";


            foreach (var item in File.ReadLines(file))
            {
                var parameters = item.Split(',');

                elevatorControl.QueueUsers(int.Parse(parameters[0]), int.Parse(parameters[1]), double.Parse(parameters[2]));
            }


            while (true)
            {
                var user = Console.ReadLine();

                var parameters = user.Split(',');

                elevatorControl.QueueUsers(int.Parse(parameters[0]), int.Parse(parameters[1]), double.Parse(parameters[2]));
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
