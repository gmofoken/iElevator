using ElevatorControl.Interfaces;
using ElevatorControlSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Interfaces
{
    public interface IElevatorControlUnit
    {
        void RegisterObserver(int elevatorId, IButtonPressObserver observer);

        void UnregisterObserver(int elevatorId, IButtonPressObserver observer);

        void ButtonPressed(int elevatorId, int floor);
    }
}
