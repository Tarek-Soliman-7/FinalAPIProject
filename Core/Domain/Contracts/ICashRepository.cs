using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICashRepository
    {
        //Get
        Task<string?> GetAsync(string cashKey);
        //Set
        Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive);
    }
}
