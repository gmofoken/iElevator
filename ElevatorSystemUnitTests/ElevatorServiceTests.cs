using Elevator;
using Elevator.DTOs;
using Elevator.Enums;
using Elevator.Services;
using ElevatorControl;
using NUnit.Framework;
using System;
using System.Linq;


namespace ElevatorSystemUnitTests
{
    public class ElevatorServiceTests
    {
        private readonly ElevatorService _elevatorService = ElevatorService.Instance;

        public ElevatorServiceTests()
        {
            _elevatorService.SetNumberOfFloors(20);
        }

        [Test]
        public void SetUpEnvironment()
        {
            //ASSERT
            Assert.IsNotNull(_elevatorService);
        }

        [Test]
        public void HasActiveElevators()
        {
            //ASSERT
            Assert.IsTrue(_elevatorService.ActiveElevators().Count == 2);
        }

        [Test]
        public void CallNearestElevators()
        {
            //ARRANGE
            int floor = 4;
            var request = new ElevatorRequest() { Direction = Direction.Up, Floor = floor };

            //ACT
            //var assignedElevator = _elevatorService.CallElevator(request);
            var expectedDistance = 0;
            var elevators = _elevatorService.ActiveElevators();

            elevators.ForEach(elevator =>
            {
                expectedDistance = Math.Abs(elevator.CurrentFLoor() - floor);
            });

            //var distance = Math.Abs(assignedElevator.Item2 - floor);



            ////ASSERT
            //Assert.AreNotEqual(distance, expectedDistance);
        }

        [Test]
        public void CallElevatorFromDifferentDirections()
        {
            //ARRANGE
            int floor = 4;
            var request = new ElevatorRequest() { Direction = Direction.Up, Floor = 1 };
            var request2 = new ElevatorRequest() { Direction = Direction.Up, Floor = 20 };

            //ACT
            //var assignedElevator1 = _elevatorService.CallElevator(request);
            //var assignedElevator2 = _elevatorService.CallElevator(request2);

            ////return Math.Abs(_currentFloor - requestedFloor);



            ////ASSERT
            //Assert.AreNotEqual(assignedElevator1.Item2, assignedElevator2.Item2);
        }
    }
}