using AutoMapper;
using Domain.Contracts;
using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceManger(IUnitOfWork _unitOfWork,IMapper _mapper) : IServiceManger
    {
        private readonly Lazy<IProductService> _productService=
            new Lazy<IProductService>(()=>new ProductService(_unitOfWork,_mapper));
        public IProductService ProductService =>_productService.Value;
    }
}
