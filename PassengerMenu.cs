using Ride_sharing_system.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;

namespace Ride_sharing_system.Menus
{
    class PassengerMenu
    {
        public static void Show(Passenger passenger)
        {
            int option = 0;
            while (option != 4)
            {
                Console.WriteLine("Passenger Menu:");
                Console.WriteLine("1. View Balance");
                Console.WriteLine("2. View Profile");
                Console.WriteLine("3. Deposit funds");
                Console.WriteLine("4. Request Ride");
                Console.WriteLine("5. Ride History");
                Console.WriteLine("6. Logout");
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
                        AddFunds(passenger);
                        break;
                    case 4:
                        RequestRide(passenger);
                        break;
                    case 5:
                        ViewRideHistory(passenger);
                        break;
                    case 6:
                        Console.WriteLine("Logging out");
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
        
        public static void ViewBalance(Passenger passenger)
        {
            if (!File.Exists("passengers.json"))
            {
                Console.WriteLine("No passengers available");
                return;
            }

            string json = File.ReadAllText("passengers.json");
            List<Passenger> users = JsonSerializer.Deserialize<List<Passenger>>(json) ?? new List<Passenger>();

            var user = users.FirstOrDefault(p => p.Email == passenger.Email);

            if (user != null)
            {
                Console.WriteLine($"Available Balance: {passenger.Balance}");
                PassengerMenu.Show(passenger);
                return;
            }
        }

        public static void AddFunds(Passenger passenger)
        {
            if (!File.Exists("passengers.json"))
            {
                Console.WriteLine("No passengers available");
                return;
            }
            
            string json = File.ReadAllText("passengers.json");
            List<Passenger> users = JsonSerializer.Deserialize<List<Passenger>>(json) ?? new List<Passenger>();

            var user = users.FirstOrDefault(p => p.Email == passenger.Email);

            if (user != null)
            {
                Console.Write("Enter amount to add to balance :");
                decimal funds = Convert.ToDecimal(Console.ReadLine());
                user.AddFunds(funds);
                Console.WriteLine($"{funds} successfully added");
                Console.WriteLine("Current balance: R" + user.Balance);
                File.WriteAllText("passengers.json", JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
                PassengerMenu.Show(user);
                return;
            }
        }
    }
}
