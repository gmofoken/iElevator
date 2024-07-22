using Elevator.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace Elevator.Models
{
    public class Elevators : ElevatorType, INotifyPropertyChanged
    {
        public Elevators(ElevatorTypeEnum type) : base (type)
        { 
        }

        private Load load { get; set; } = new Load();
        private int currentFloor { get; set; }
        private Direction currentDirection { get; set; }

        public Load Load {  get { return load; } set { load = value; }  }
        public int CurrentFloor { get { return currentFloor; } set { currentFloor = value; } }
        public Direction CurrentDirection { get { return currentDirection; } set { currentDirection = value; } }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Load
    {

        public List<User> Users { get; set; } = new List<User>();
        public double Weight { get; set; } = 00;
        public int Capacity { get; set; } = 0;
    }

    public class ElevatorType
    {
        private readonly int maxCapacity = 0;
        private readonly double maxWeight = 0;
        private readonly int speed = 0;

        public ElevatorType(ElevatorTypeEnum type)
        {
            if (type == ElevatorTypeEnum.Express)
            {
                maxWeight = 600;
                maxCapacity = 5;
                speed = 3000;
            }
            else if (type == ElevatorTypeEnum.Normal)
            {
                maxWeight = 1200;
                maxCapacity = 10;
                speed = 6000;
            }
            else if (type == ElevatorTypeEnum.Large)
            {
                maxWeight = 2400;
                maxCapacity = 15;
                speed = 8000;
            }
        }
        

        public int MaxCapacity { get { return maxCapacity; } }
        public double MaxWeight { get { return maxWeight; } }
        public int Speed { get { return speed; } }
    }
}
