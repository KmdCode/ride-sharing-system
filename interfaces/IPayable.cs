using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ride_sharing_system.interfaces
{
    internal interface IPayable
    {
        void AddFunds(decimal funds);
    }
}
