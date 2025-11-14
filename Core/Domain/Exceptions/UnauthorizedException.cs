using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message= $"Invalid email or password"):base(message)
        {
            
        }
    }
}
