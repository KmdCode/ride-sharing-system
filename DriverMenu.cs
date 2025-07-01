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
                Console.WriteLine("4. Complete Ride");
                Console.WriteLine("5. Logout");
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
                        CompleteRide(driver);
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

        public static void ViewAvailableRides(Driver driver)
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

            Console.WriteLine("Available Rides:");
            for (int i = 0; i < pendingRides.Count; i++)
            {
                var ride = pendingRides[i];
                Console.WriteLine($"[{i + 1}] Pickup: {ride.PickupLocation}, Destination: {ride.Destination}, Distance: {ride.DistanceKm}km, Cost: R{ride.Cost}, Passenger: {ride.PassengerEmail}");
            }

            Console.Write("Enter ride number to accept (or 0 to cancel): ");

            int selection = Convert.ToInt32(Console.ReadLine());
            
            if (selection > 0 && selection <= pendingRides.Count)
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

        public static void CompleteRide(Driver driver)
        {
            if (!File.Exists("rides.json"))
            {
                Console.WriteLine("No rides available.");
                return;
            }

            if (!File.Exists("passengers.json"))
            {
                Console.WriteLine("No rides available.");
                return;
            }

            string ridesJson = File.ReadAllText("rides.json");
            List<Ride> rides = JsonSerializer.Deserialize<List<Ride>>(ridesJson) ?? new List<Ride>();

            string passengerJson = File.ReadAllText("passengers.json");
            List<Passenger> passengers = JsonSerializer.Deserialize<List<Passenger>>(passengerJson) ?? new List<Passenger>();


            string driversJson = File.ReadAllText("drivers.json");
            List<Driver> drivers = JsonSerializer.Deserialize<List<Driver>>(passengerJson) ?? new List<Driver>();

            var newDriver = drivers.FirstOrDefault(r => r.Email == driver.Email);

            var acceptedRides = rides.Where(r => r.Status == "Accepted" && r.AssignedDriverEmail == driver.Email).ToList();

            if (acceptedRides.Count == 0)
            {
                Console.WriteLine("No pending rides found.");
                return;
            }

            Console.WriteLine("Available Rides:");
            for (int i = 0; i < acceptedRides.Count; i++)
            {
                var ride = acceptedRides[i];
                Console.WriteLine($"[{i + 1}] Pickup: {ride.PickupLocation}, Destination: {ride.Destination}, Distance: {ride.DistanceKm}km, Cost: R{ride.Cost}, Passenger: {ride.PassengerEmail}");
            }

            Console.Write("Enter ride number to complete (or 0 to cancel): ");

            int selection = Convert.ToInt32(Console.ReadLine());

            if (selection > 0 && selection <= acceptedRides.Count)
            {
                
                var selectedRide = acceptedRides[selection - 1];
                var passenger = passengers.FirstOrDefault(r => r.Email == selectedRide.PassengerEmail);
                passenger.Balance -= selectedRide.Cost;
                selectedRide.Status = "Completed";
                newDriver.AddFunds(passenger.Balance);


                var indexInOriginal = rides.FindIndex(r => r.RequestedAt == selectedRide.RequestedAt && r.PassengerEmail == selectedRide.PassengerEmail);
                if (indexInOriginal >= 0)
                    rides[indexInOriginal] = selectedRide;

                File.WriteAllText("passengers.json", JsonSerializer.Serialize(passengers, new JsonSerializerOptions { WriteIndented = true }));
                File.WriteAllText("rides.json", JsonSerializer.Serialize(rides, new JsonSerializerOptions { WriteIndented = true }));
                File.WriteAllText("drivers.json", JsonSerializer.Serialize(drivers, new JsonSerializerOptions { WriteIndented = true }));

                Console.WriteLine("Ride completed successfully!");
            }
            else
            {
                Console.WriteLine("Cancelled or invalid selection.");
            }
        }

    }
}
