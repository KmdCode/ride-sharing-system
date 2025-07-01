using Ride_sharing_system.classes;
using Ride_sharing_system.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Ride_sharing_system
{
    internal class Program
    {
        public static List<Passenger> passengers = new List<Passenger>();
        public static List<Driver> drivers = new List<Driver>();

        static void Main(string[] args)
        {
            Console.WriteLine("Select Option:");
            Console.WriteLine("1. Driver Registration");
            Console.WriteLine("2. Passenger Registration");
            Console.WriteLine("3. Login");
            Console.WriteLine("4. Logout");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    RegisterDriver();
                    break;
                case 2:
                    RegisterPassenger();
                    break;
                case 3:
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }
        }

        public static void RegisterPassenger()
        {
            try
            {
                if (File.Exists("passengers.json"))
                {
                    string json = File.ReadAllText("passengers.json");
                    passengers = JsonSerializer.Deserialize<List<Passenger>>(json) ?? new List<Passenger>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                passengers = new List<Passenger>();
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Passenger temp = new Passenger(name, email, password);
            passengers.Add(temp);
            Console.WriteLine("Passenger registered");

            File.WriteAllText("passengers.json", JsonSerializer.Serialize(passengers, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void RegisterDriver()
        {
            try
            {
                if (File.Exists("drivers.json"))
                {
                    string json = File.ReadAllText("drivers.json");
                    drivers = JsonSerializer.Deserialize<List<Driver>>(json) ?? new List<Driver>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading driver data: " + ex.Message);
                drivers = new List<Driver>();
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Driver temp = new Driver(name, email, password);
            drivers.Add(temp);
            Console.WriteLine("Driver registered");

            File.WriteAllText("drivers.json", JsonSerializer.Serialize(drivers, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void Login()
        {

            try
            {
                if (File.Exists("passengers.json"))
                {
                    string passengerJson = File.ReadAllText("passengers.json");
                    passengers = JsonSerializer.Deserialize<List<Passenger>>(passengerJson) ?? new List<Passenger>();
                }

                if (File.Exists("drivers.json"))
                {
                    string driverJson = File.ReadAllText("drivers.json");
                    drivers = JsonSerializer.Deserialize<List<Driver>>(driverJson) ?? new List<Driver>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var passenger = passengers.FirstOrDefault(p => p.Email == email && p.Password == password);
            if (passenger != null)
            {
                Console.WriteLine($"Welcome Passenger: {passenger.Name}");
                PassengerMenu.Show(passenger);
                return;
            }

            var driver = drivers.FirstOrDefault(d => d.Email == email && d.Password == password);
            if (driver != null)
            {
                Console.WriteLine($"Welcome Driver: {driver.Name}");
                DriverMenu.Show(driver);
                return;
            }

            Console.WriteLine("Invalid email or password.");
        }


    }
}
