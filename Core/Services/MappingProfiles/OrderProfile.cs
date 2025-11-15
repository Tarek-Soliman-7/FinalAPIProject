using ShippingAddress = Domain.Entities.OrderModule.Address;
using IdentityAddress = Domain.Entities.IdentityModule.Address;
namespace Services.MappingProfiles
{
    internal class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, AddressDto>().ReverseMap();
            CreateMap<IdentityAddress,AddressDto>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMehtodResult>()
                .ForMember(dest=>dest.Cost,op => op.MapFrom(src=>src.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest=> dest.ProductId , options=>options.MapFrom(src=>src.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.Product.PictureUrl));
            CreateMap<Order, OrderResult>()
                .ForMember(dest => dest.Status, op => op.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, op => op.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, op => op.MapFrom(src => src.DeliveryMethod.Price+src.SubTotal));



        }
    }
}
