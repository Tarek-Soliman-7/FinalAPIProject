using Services.Abstraction.Contracts;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDelegate(Func<IProductService> _productFactory,
        Func<IOrderService> _orderFactory, Func<IBasketService> _basketFactory,
        Func<IPaymentService> _paymentFactory, Func<IAuthenticationService> _authFactory
        , Func<ICashService> _cashFactory) : IServiceManger
    {
        public IProductService ProductService => _productFactory.Invoke();

        public IBasketService BasketService => _basketFactory.Invoke();

        public IAuthenticationService AuthenticationService => _authFactory.Invoke();

        public IOrderService OrderService => _orderFactory.Invoke();

        public IPaymentService PaymentService => _paymentFactory.Invoke();

        public ICashService CashService => _cashFactory.Invoke();
    }
}
