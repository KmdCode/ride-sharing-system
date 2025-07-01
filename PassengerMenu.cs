using Ride_sharing_system.classes;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;

namespace Ride_sharing_system.Menus
{
    class PassengerMenu
    {
        public static void Show(Passenger passenger)
        {
            int option = 0;
            while (option != 4)
            {
                Console.WriteLine("\nPassenger Menu:");
                Console.WriteLine("1. View Balance");
                Console.WriteLine("2. View Profile");
                Console.WriteLine("3. Request Ride");
                Console.WriteLine("4. Ride History");
                Console.WriteLine("5. Logout");
                Console.Write("Choose option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.WriteLine($"Balance: R{passenger.Balance:F2}");
                        break;
                    case 2:
                        Console.WriteLine($"Name: {passenger.Name}\nEmail: {passenger.Email}");
                        break;
                    case 3:
                        RequestRide(passenger);
                        break;
                    case 4:
                        ViewRideHistory(passenger);
                        break;
                    case 5:
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        private static void RequestRide(Passenger passenger)
        {
            Console.Write("Enter Pickup Location: ");
            string pickup = Console.ReadLine();

            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();

            Ride newRide = new Ride(passenger.Email, pickup, destination);

            List<Ride> rides = new List<Ride>();

            if (File.Exists("rides.json"))
            {
                string json = File.ReadAllText("rides.json");
                rides = JsonSerializer.Deserialize<List<Ride>>(json) ?? new List<Ride>();
            }

            rides.Add(newRide);

            File.WriteAllText("rides.json", JsonSerializer.Serialize(rides, new JsonSerializerOptions { WriteIndented = true }));

            Console.WriteLine("Ride requested successfully");
        }

        public static void ViewRideHistory(Passenger passenger)
        {
            if (!File.Exists("rides.json"))
            {
                Console.WriteLine("No rides available.");
                return;
            }

            string json = File.ReadAllText("rides.json");
            List<Ride> rides = JsonSerializer.Deserialize<List<Ride>>(json) ?? new List<Ride>();

            var myRides = rides.Where(r => r.PassengerEmail == passenger.Email).ToList();

            Console.WriteLine("Ride History:");
            for (int i = 0; i < myRides.Count; i++)
            {
                var ride = myRides[i];
                Console.WriteLine($"[{i + 1}] Pickup: {ride.PickupLocation}, Destination: {ride.Destination}, Distance: {ride.DistanceKm}km, Cost: R{ride.Cost}");
            }
        }
    }
}
