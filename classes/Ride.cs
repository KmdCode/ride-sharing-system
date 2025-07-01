using System;

namespace Ride_sharing_system.classes
{
    class Ride
    {
        public string PassengerEmail { get; set; }
        public string PickupLocation { get; set; }
        public string Destination { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = "Pending";

        public double DistanceKm { get; set; }
        public double Cost { get; set; }
        public string AssignedDriverEmail { get; set; }

        private static readonly Random rand = new Random();

        public Ride() { }

        public Ride(string passengerEmail, string pickup, string destination)
        {
            PassengerEmail = passengerEmail;
            PickupLocation = pickup;
            Destination = destination;
            RequestedAt = DateTime.Now;
            Status = "Pending";
            DistanceKm = rand.Next(5, 21);
            Cost = DistanceKm * 2;
            AssignedDriverEmail = null;
        }
    }
}
