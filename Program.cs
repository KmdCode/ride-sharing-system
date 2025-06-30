using Ride_sharing_system.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("3. Log");
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
            }
        }

        public static void RegisterPassenger()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Passenger temp = new Passenger(name, email, password);
            
            passengers.Add(temp);
            Console.WriteLine("User registered");

            File.AppendAllText("users.json", JsonSerializer.Serialize(passengers, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void RegisterDriver()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Driver temp = new Driver(name, email, password);

            drivers.Add(temp);
            Console.WriteLine("Driver registered");

            File.AppendAllText("users.json", JsonSerializer.Serialize(drivers, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void Login()
        {
            string data = File.ReadAllText("users.json");
            List<User> users = JsonSerializer.Deserialize<List<User>>(data);
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Name}, {user.Email}");
            }

        }

        public void PassengerMenu(Passenger passenger)
        {
            int option = 1;

            switch (option)
            {
                case 1:
                    Console.WriteLine("View Balance");
                    break;
            }
        }
    }
}
