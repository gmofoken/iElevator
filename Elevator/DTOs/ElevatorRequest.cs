using Elevator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator.DTOs
{
    public class ElevatorRequest
    {
        public int Floor { get; set; }
        public Direction Direction { get; set; }
    }
}
