using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;
using Order = Domain.Entities.OrderModule.Order; 
namespace Services.Implementations
{
    public class PaymentService(IConfiguration _configuration,IBasketRepository _basketRepository
        ,IUnitOfWork _unitOfWork,IMapper _mapper) : IPaymentService
    {
        /*public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //0] Install stripe.net

            //1] Set up key [Secret Key] ==> stripe key
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

            //2] Get basket [By basketId] 
            var basket = await _basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId) ;

            //3] Validate Items Price ==> [basket.item.price = product.price] ==> product from db
            foreach(var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GetRepository<Product,int>().GetByIdAsync(item.Id) 
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            //4] Validate Shipping Price ==> get deliveryMethod [DeliveryMethod.Id]
            //                           ==> shippingPrice = DeliveryMethod.Price
            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;

            //5] Total ==> [subTotal + shippingPrice] ==> cent ==> * 100==> long
            //  ==>(long) ([basketItem.items.q * basketItem.items.price] + shippingPrice [DeliveryMethod.Price])*100
            var amount = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;

            //6] Create or update PaymentIntentId
            var stripeService = new PaymentIntentService();
            if(string.IsNullOrEmpty(basket.PaymentIntentId))//create
            {
                var options=new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency="USD",//dollar
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }

            //7] Save changes [update] Basket
            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            //8] Map to BasketDto ==> return
            return _mapper.Map<BasketDto>(basket);

        }*/
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

            var basket = await GetBasketAsync(basketId);

            await ValidateBasketAsync(basket);

            var amount = CalculateTotalAsync(basket);

            await CreationOrUpdatePaymentIntentAsync(basket, amount);

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);

        }

        private async Task CreationOrUpdatePaymentIntentAsync(CustomerBasket basket, long amount)
        {
            var stripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))//create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",//dollar
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }
        }

        private long CalculateTotalAsync(CustomerBasket basket)
        {
            var amount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
            return amount;
        }

        private async Task ValidateBasketAsync(CustomerBasket basket)
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery method selected");
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId)
               ?? throw new BasketNotFoundException(basketId);
        }

        public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
        {
            string endpointSecret = _configuration.GetSection("StripeSettings")["EndPointSecret"];

            var stripeEvent = EventUtility.ParseEvent(json, throwOnApiVersionMismatch: false);

            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret, throwOnApiVersionMismatch: false);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;


            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                //Change Order Payment Status ==> paymentRecieved
                await UpdatePaymentStatusRecievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                //Change Order Payment Status ==> paymentFailed
                await UpdatePaymentStatusFailedAsync(paymentIntent.Id);

            }
            // ... handle other event types
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

        }

        private async Task UpdatePaymentStatusFailedAsync(string paymentIntentId)
        {
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order =await OrderRepo
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus=OrderPaymentStatus.PaymentFailed;
                OrderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task UpdatePaymentStatusRecievedAsync(string paymentIntentId)
        {
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await OrderRepo
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
                OrderRepo.Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
