using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IdentityModule
{
    public class User:IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public Address Address { get; set; } 
    }
}
