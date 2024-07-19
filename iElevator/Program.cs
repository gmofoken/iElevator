using ElevatorControlSystem;
using System;

namespace iElevator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var elevatorControl = new ElevatorControl();
            var elevator1Display = new ElevatorDisplay();
            var elevator2Display = new ElevatorDisplay();

            // Register observers for different elevators
            elevatorControl.RegisterObserver(1, elevator1Display);
            elevatorControl.RegisterObserver(2, elevator2Display);

            // Simulate button presses
            elevatorControl.ButtonPressed(1, 3);
            elevatorControl.ButtonPressed(2, 5);

            // Unregister observer for elevator 1
            //elevatorControl.UnregisterObserver(1, elevator1Display);

            // No notification for elevator 1 after unregistering
            elevatorControl.ButtonPressed(1, 4);
            
        }
    }
}
