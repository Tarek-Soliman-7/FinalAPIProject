using Shared.Dtos.OrderModule;
using System.Collections.Generic;

namespace Services.Abstraction.Contracts
{
    public interface IOrderService
    {

        //GetById ==> Take Guid Id ==> Return OrderResult
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        //GetAllByEmail ==> Take string Email ==> Return IEnumerable<OrderResult>
        Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail);
        //CreateOrder ==> Take OrderRequest , string Email ==> Return OrderResult
        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest,string userEmail);
        //GetDeliveryMethods ==> Dont Take Para ==>  Return IEnumerable<DeliveryMehtodResult>
        Task<IEnumerable<DeliveryMehtodResult>> GetDeliveryMethodsAsync();
    }
}
