using Ride_sharing_system.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ride_sharing_system.classes
{
    internal class Passenger: User, IPayable
    {
        //public string PickUp { get; set; }
        //public string DropOff { get; set; } 
        public decimal Balance { get; set; }

        public Passenger(string Name, string Email, string Password)
        {
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
        }

        public void ViewBalance()
        {
            Console.WriteLine("Your Balance is: " + Balance);
        }

        public void AddFunds(decimal amount)
        {
            Balance += amount;
        }


    }
}
