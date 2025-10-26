using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var brands = await Repo.GetAllAsync();
            var brandsDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);
            return brandsDto;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductSpecificationParameters parameters)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var specifications = new ProductWithBrandAndTypeSpecifications(parameters);
            var products = await productRepo.GetAllAsync(specifications);
            var productsResult= _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var pageSize = productsResult.Count();
            var countSpecifications=new ProductCountSpecification(parameters);
            var totalCount = await productRepo.CountAsync(countSpecifications);
            return new PaginatedResult<ProductDto>(parameters.PageIndex, pageSize,totalCount,productsResult);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto> GetProductsByIdAsync(int Id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(Id);
            var products = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            var productResults= _mapper.Map<Product, ProductDto>(products);
            //return productResults;
            return products is null ? throw new ProductNotFoundException(Id):productResults;
        }
    }
}
