using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManger _serviceManger) : ApiController
    {
        //Create Order
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrderAsync(OrderRequest orderRequest)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManger.OrderService.CreateOrderAsync(orderRequest, userEmail);
            return Ok(order);
        }
        //Get Order By Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResult>> GetOrderByIdAsync(Guid Id)
        {
            var order = await _serviceManger.OrderService.GetOrderByIdAsync(Id);
            return Ok(order);
        }

        //Get All Order By Email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetAllOrdersByEmailAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManger.OrderService.GetOrdersByEmailAsync(userEmail);
            return Ok(orders);
        }

        //Get Delivery Methods
        [HttpGet("{DeliveryMethods}")]
        public async Task <ActionResult<IEnumerable<DeliveryMehtodResult>>> GetDeliveryMehtodAsync()
        {
            var deliveryMethods = await _serviceManger.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}
