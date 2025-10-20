using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class ProductsController(IServiceManger _serviceManger) : ControllerBase
    {
        // EndPoint 1 ==> GetAllProducts
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProductsAsync([FromQuery]ProductSpecificationParameters parameters)
        => Ok(await _serviceManger.ProductService.GetAllProductsAsync(parameters));

        // EndPoint 2 ==> GetAllBrands
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        => Ok(await _serviceManger.ProductService.GetAllBrandsAsync());

        // EndPoint 3 ==> GetAllTypes
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        => Ok(await _serviceManger.ProductService.GetAllTypesAsync());

        // EndPoint 4 ==> GetProductById
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
     => Ok(await _serviceManger.ProductService.GetProductsByIdAsync(id));


    }
}
