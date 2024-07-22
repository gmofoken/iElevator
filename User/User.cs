using System;
using Users.Enums;

namespace Users
{
    public class User
    {
        public string Id { get; set; }
        public int DestinationFloor { get; set; }   
        public int CurrentFloor;
        public double Weight { get; set; }

        public UserAction CurrentAction { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }



    }
}
