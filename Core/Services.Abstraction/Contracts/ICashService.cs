using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface ICashService
    {
        Task<string?> GetAsync(string cashKey);
        Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive);
    }
}
