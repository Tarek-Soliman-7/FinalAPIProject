using AutoMapper;
using Domain.Entities.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _configurations) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return string.Empty;
            }
            return $"{_configurations.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
