namespace Services.Implementations
{
    public class ServiceManger(IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IBasketRepository _basketRepository,
        UserManager<User> _userManager ,IOptions<JwtOptions> _options,
        IConfiguration _configuration) : IServiceManger
    {
        private readonly Lazy<IProductService> _productService=
            new Lazy<IProductService>(()=>new ProductService(_unitOfWork,_mapper));
        private readonly Lazy<IBasketService> _basketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepository,_mapper));
        private readonly Lazy<IAuthenticationService> _authenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager,_options,_mapper));
        private readonly Lazy<IOrderService> _orderService=
            new Lazy<IOrderService>(() => new OrderService(_mapper, _basketRepository, _unitOfWork));
        private readonly Lazy<IPaymentService> _paymentService=
            new Lazy<IPaymentService>(() => new PaymentService(_configuration,_basketRepository,_unitOfWork,_mapper));
        public IProductService ProductService =>_productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService paymentService => _paymentService.Value;
    }
}
