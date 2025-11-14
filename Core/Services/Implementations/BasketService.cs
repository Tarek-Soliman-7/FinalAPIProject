using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket=_mapper.Map<CustomerBasket>(basketDto);
           var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return createdOrUpdatedBasket is null ? throw new Exception("Cannot Create Or Update The Basket"):
                _mapper.Map<BasketDto>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string Id)
        => await _basketRepository.DeleteBasketAsync(Id);

        public async Task<BasketDto> GetBasketAsync(string Id)
        {
            var basket = await _basketRepository.GetBasketAsync(Id);
            return basket is null ? throw new BasketNotFoundException(Id) : 
                _mapper.Map<BasketDto>(basket);
        }
    }
}
