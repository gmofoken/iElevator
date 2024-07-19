using ElevatorControl;
using NUnit.Framework;
using System;
using System.Linq;


namespace ElevatorSystemUnitTests
{
    public class ElevatorControlTests
    {

        [Test]
        public void CreateElevators()
        {
            //ARRANGE
            int numberOfElevators = 3;
            var elevatorControl = new ElevatorControlUnit(numberOfElevators);

            //ASSERT
            Assert.AreEqual(numberOfElevators, elevatorControl.GetElevators().Count);
        }

        [Test]
        public void FindClosestElevator()
        {
            //ARRANGE
            int numberOfElevators = 3;
            var elevatorControl = new ElevatorControlUnit(numberOfElevators);
            var elevator = elevatorControl.GetElevators().OrderBy(e => e.CurrentFloor).FirstOrDefault();

            //ACT
            int callFloor = 2;

            callFloor = (elevator.CurrentFloor == 2) ? 1 : 2;


            var closestElevator = elevatorControl.FindClosestElevator(elevatorControl.GetElevators(), callFloor);



            //ASSERT
            Assert.AreEqual(elevator.id , closestElevator.id);
        }
    }
}