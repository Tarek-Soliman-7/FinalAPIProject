using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
   
    public class BasketController(IServiceManger _serviceManger) : ApiController
    {
        //Get
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketAsync(string Id)
            => Ok(await _serviceManger.BasketService.GetBasketAsync(Id));
        //Post
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync( BasketDto basket)
            =>Ok (await _serviceManger.BasketService.CreateOrUpdateBasketAsync(basket));
        //Delete
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteBasket(string Id)
        {
            await _serviceManger.BasketService.DeleteBasketAsync(Id);
            return NoContent();
        }

    }
}
