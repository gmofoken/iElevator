using Elevator;
using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Services;
using ElevatorControl.Interfaces;
using ElevatorControlSystem.Interfaces;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Users;
using Users.Enums;

namespace ElevatorControl
{
    

    public class ElevatorControlUnit 
    {
        private Dictionary<int, List<IButtonPressObserver>> _elevatorObservers = new Dictionary<int, List<IButtonPressObserver>>();

        private readonly ElevatorService _elevatorService; 

        

        public ElevatorControlUnit(int floors, List<ElevatorTypeEnum> elevators)
        {
            //var elevators = new List<ElevatorTypeEnum>() { ElevatorTypeEnum.Express, ElevatorTypeEnum.Normal, ElevatorTypeEnum.Large };
            _elevatorService = new ElevatorService(elevators, 20);
        }

        public void QueueUsers(int currentFloor, int targetFloor, double weight)
        {
            var user = new User()
            {
                CurrentFloor = currentFloor,
                DestinationFloor = targetFloor,
                Weight = weight,
                CurrentAction = UserAction.Waiting
            };
            _elevatorService.AddUserToQueue(user);

            var request = new ElevatorRequest()
            {
                Floor = currentFloor
            };

            _elevatorService.CallElevator(request);
        }

    }

    

    public class ElevatorDisplay : IButtonPressObserver
    {
        public void HandleButtonPress(int floor)
        {
            Console.WriteLine($"Elevator Display: Going to floor {floor}");
        }
    }

    
}
