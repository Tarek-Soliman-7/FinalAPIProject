using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class OrderNotFoundException:NotFoundException
    {
        public OrderNotFoundException(Guid id):base($"Order With Id {id} NotFound")
        {
            
        }
        public OrderNotFoundException(string userEmail) : base($"Order With Email {userEmail} NotFound")
        {

        }
    }
}
