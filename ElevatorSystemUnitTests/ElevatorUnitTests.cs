using Elevator;
using Elevator.DTOs;
using Elevator.Enums;
using ElevatorControl;
using NUnit.Framework;
using System;
using System.Linq;


namespace ElevatorSystemUnitTests
{
    public class ElevatorUnitTests
    {

        [Test]
        public void CalculateDistance()
        {
            //ARRANGE
            int floor = new Random().Next(1, 10);
            var elevator = new ElevatorUnit(new Random().Next(1,10) ,1);

            //ACT
            var distance  = elevator.CalculateDistance(floor);
            var compare = Math.Abs(elevator.CurrentFLoor() - floor);

            //ASSERT
            Assert.AreEqual(distance, compare);
        }

        [Test]
        public void CallElevator()
        {
            //ARRANGE
            int floor = new Random().Next(1, 10);
            var elevator = new ElevatorUnit(10, 1);
            var request1 = new ElevatorRequest() { Direction = Direction.Up, Floor = floor };
            var request2 = new ElevatorRequest() { Direction = Direction.Up, Floor = floor - 1 };


            //ACT
            elevator.QueueStops(request1.Floor);
            elevator.QueueStops(request2.Floor);

            //ASSERT
            Assert.AreEqual(elevator.PlannedStops(), 2);
        }

        [Test]
        public void WillPassRequestedFloor()
        {
            //ARRANGE
            var elevator = new ElevatorUnit(20, 1);
            var request1 = new ElevatorRequest() { Direction = Direction.Up, Floor = new Random().Next(1, 5) };
            var request2 = new ElevatorRequest() { Direction = Direction.Up, Floor = new Random().Next(6, 10) };


            //ACT
            var floor1 = elevator.WillPassRequestedFloor(request1);
            var floor2 = elevator.WillPassRequestedFloor(request2);

            //ASSERT
            Assert.AreEqual(floor1, floor2);
        }

        [Test]
        public void WillPassRequestedFloorDiffDirections()
        {
            //ARRANGE
            var elevator = new ElevatorUnit(10, 1);
            var request1 = new ElevatorRequest() { Direction = Direction.Up, Floor = new Random().Next(11, 20) };
            var request2 = new ElevatorRequest() { Direction = Direction.Up, Floor = new Random().Next(1, 10) };


            //ACT
            var floor1 = elevator.WillPassRequestedFloor(request1);
            var floor2 = elevator.WillPassRequestedFloor(request2);

            //ASSERT
            Assert.AreNotSame(floor1, floor2);
        }
    }
}