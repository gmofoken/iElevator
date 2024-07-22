using ElevatorControlSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using Elevator;
using ElevatorControl;
using System.IO;
using System.Threading.Tasks;
using Users;
using Elevator.Enums;
using System.Linq;

namespace iElevator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var elevators = GetElevators();

            int floors = SetNumberOfFloors();


            var elevatorControl = new ElevatorControlUnit(floors, elevators);

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

        }

        public static List<ElevatorTypeEnum>  GetElevators()
        {
            var elevators = new List<ElevatorTypeEnum>();

            bool valid = true;

            while (valid)
            {
                Console.WriteLine("Please Enter the type of elevators to simulate in below format");
                Console.WriteLine("Each elevator is represented by a character and the number of comma separated characters will determine number of elevators");
                Console.WriteLine("E = Express, L = Large,N = Normal");
                Console.WriteLine("E,L,N");

                elevators.Clear();

                var elevatorsInput = Console.ReadLine();
                var parameters = elevatorsInput.Split(',');

                

                foreach (var item in parameters)
                {
                    if (item.Length == 1 && new[] { "E", "L", "N" }.Contains(item))
                    {
                        if (item == "E")
                            elevators.Add(ElevatorTypeEnum.Express);
                        if (item == "L")
                            elevators.Add(ElevatorTypeEnum.Large);
                        if (item == "N")
                            elevators.Add(ElevatorTypeEnum.Normal);

                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                        valid = false;
                    }
                }

            }

            return elevators;
        } 

        public static int SetNumberOfFloors()
        {
            int floors = 0;

            while (true)
            {
                Console.WriteLine("Please enter the number of floors");
                var floorInput = Console.ReadLine();

                try
                {
                    floors = int.Parse(floorInput);
                }
                catch (Exception)
                {
                    continue;
                }
                break;
            }
            return floors;
        }
    }

    
}
