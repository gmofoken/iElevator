using Elevator.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace ElevatorControl.DTOs
{
    public class Floor
    {
        public int FloorID { get; private set; }
        public bool IsThereGroupGoingUp { get; set; }
        public bool IsThereGroupGoingDown { get; set; }
        public Queues Queues { get; private set; } = new Queues();


        public Floor(int id) 
        {
            FloorID = id;
        }
    }

    public class Queues
    {
        public Queue<User> Up { get; set; } = new Queue<User>();
        public Queue<User> Down { get; set; } = new Queue<User>();
    }
}
