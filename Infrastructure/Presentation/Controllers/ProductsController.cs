using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    
    public class ProductsController(IServiceManger _serviceManger) : ApiController
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

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]



        // EndPoint 4 ==> GetProductById
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
     => Ok(await _serviceManger.ProductService.GetProductsByIdAsync(id));


    }
}
