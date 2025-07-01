using Ride_sharing_system.classes;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;

namespace Ride_sharing_system.Menus
{
    class DriverMenu
    {
        public static void Show(Driver driver)
        {
            int option = 0;
            while (option != 4)
            {
                Console.WriteLine("Driver Menu:");
                Console.WriteLine("1. View Balance");
                Console.WriteLine("2. View Profile");
                Console.WriteLine("3. View Available Rides");
                Console.WriteLine("4. Logout");
                Console.Write("Choose option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.WriteLine($"Balance: R{driver.Balance:F2}");
                        break;
                    case 2:
                        Console.WriteLine($"Name: {driver.Name}\nEmail: {driver.Email}");
                        break;
                    case 3:
                        ViewAvailableRides(driver);
                        break;
                    case 4:
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        private static void ViewAvailableRides(Driver driver)
        {
            if (!File.Exists("rides.json"))
            {
                Console.WriteLine("No rides available.");
                return;
            }

            string json = File.ReadAllText("rides.json");
            List<Ride> rides = JsonSerializer.Deserialize<List<Ride>>(json) ?? new List<Ride>();

            var pendingRides = rides.Where(r => r.Status == "Pending").ToList();

            if (pendingRides.Count == 0)
            {
                Console.WriteLine("No pending rides found.");
                return;
            }

            Console.WriteLine("\nAvailable Rides:");
            for (int i = 0; i < pendingRides.Count; i++)
            {
                var ride = pendingRides[i];
                Console.WriteLine($"[{i + 1}] Pickup: {ride.PickupLocation}, Destination: {ride.Destination}, Distance: {ride.DistanceKm}km, Cost: R{ride.Cost}, Passenger: {ride.PassengerEmail}");
            }

            Console.Write("\nEnter ride number to accept (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= pendingRides.Count)
            {
                var selectedRide = pendingRides[selection - 1];
                selectedRide.Status = "Accepted";
                selectedRide.AssignedDriverEmail = driver.Email;

                
                var indexInOriginal = rides.FindIndex(r => r.RequestedAt == selectedRide.RequestedAt && r.PassengerEmail == selectedRide.PassengerEmail);
                if (indexInOriginal >= 0)
                    rides[indexInOriginal] = selectedRide;

                File.WriteAllText("rides.json", JsonSerializer.Serialize(rides, new JsonSerializerOptions { WriteIndented = true }));

                Console.WriteLine("Ride accepted successfully!");
            }
            else
            {
                Console.WriteLine("Cancelled or invalid selection.");
            }
        }

    }
}
