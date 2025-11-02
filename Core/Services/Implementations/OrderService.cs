using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.OrderModule;

namespace Services.Implementations
{
    internal class OrderService(IMapper _mapper
        ,IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            //1] Map addressDto to address
            var address = _mapper.Map<Address>(orderRequest.ShippingAddress);
            //2] GetOrderItem ==> BasketId ==> Basket ==> BasketItem [Id]
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);
            var orderItems=new List<OrderItem>();
            foreach(var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(product, item));
            }
            //3] GetDeliveryMethod ==> DeliveryMethodId ==> DB
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderRequest.DeliveryMehtodId)
                ??throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMehtodId);
            //4] calc SubTotal ==> OrderItem ==> OrderItem.Q * OrderItem.Price
            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);
            //5] Create obj from order ==> params , Add DB , Save Chages
            var orderToCreate = new Order(userEmail, address, orderItems, deliveryMethod, subTotal);
            await _unitOfWork.GetRepository<Order,Guid>().AddAsync(orderToCreate);
            await _unitOfWork.SaveChangesAsync();
            //6] Map Order to OrderResult
            return _mapper.Map<OrderResult >(orderToCreate);  
        }

        private OrderItem CreateOrderItem(Product product, BasketItem item)
        {
            var productInOrderItem= new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);
            return new OrderItem(productInOrderItem, product.Price, item.Quantity);
            
        }


        public async Task<IEnumerable<DeliveryMehtodResult>> GetDeliveryMethodsAsync()
        {
            var deliveryMethod =await _unitOfWork.GetRepository<DeliveryMethod,int>()
                .GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMehtodResult>>(deliveryMethod);

        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludesSpecifications(id))
                ?? throw new OrderNotFoundException(id);
            return _mapper.Map<OrderResult>(order);
        }
        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>()
               .GetAllAsync(new OrderWithIncludesSpecifications(userEmail))
               ?? throw new OrderNotFoundException(userEmail);
            return _mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
