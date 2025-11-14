using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class UserNotFoundException:NotFoundException
    {
        public UserNotFoundException(string userEmail):base($"User with email: {userEmail} not found")
        {
            
        }
    }
}
