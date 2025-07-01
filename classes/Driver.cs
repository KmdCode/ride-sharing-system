using Ride_sharing_system.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ride_sharing_system.classes
{
    class Driver: User, IPayable
    {
        public decimal Balance {  get; set; }

        public Driver(string Name, string Email, string Password)
        {
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
            Balance = 0;
            Role = "driver";
        }

        public void AddFunds(decimal amount)
        {
            Balance += amount;
        }

    }
}
