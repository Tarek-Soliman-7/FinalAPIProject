using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.Dtos.ProductModule;

namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            
            CreateMap<Product, ProductDto>()
                .ForMember(dist => dist.BrandName, options => options.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(dist=>dist.PictureUrl,options=>options.MapFrom<PictureUrlResolver>());

            CreateMap<ProductType, TypeDto>();
            CreateMap<ProductBrand, BrandDto>();
           



        }
    }
}
