using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IBasketService
    {
        //Get
        Task<BasketDto> GetBasketAsync(string Id);
        //Delete
        Task<bool> DeleteBasketAsync(string Id);
        //CreateOrUpdate
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);
    }
}
