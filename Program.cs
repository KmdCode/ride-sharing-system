using Ride_sharing_system.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ride_sharing_system
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Passenger> passengers = new List<Passenger>();
            do
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

                foreach(Passenger passenger in passengers)
                {
                    Console.WriteLine(passenger.Name);
                }
            } while (true);

            
        }

        

        
    }
}
