using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControl.Interfaces
{
    public interface IButtonPressObserver
    {
        void HandleButtonPress(int floor);
    }
}
