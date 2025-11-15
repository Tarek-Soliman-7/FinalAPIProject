using Product = Domain.Entities.ProductModule.Product;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product,int>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationParameters parameters)
            :base(p=>(!parameters.TypeId.HasValue || p.TypeId==parameters.TypeId)&&
                     (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)&&
                     (string.IsNullOrEmpty(parameters.Search)||p.Name.ToLower().Contains(parameters.Search.ToLower())))
        {
            AddIncludes(p=>p.ProductBrand);
            AddIncludes(p=>p.ProductType);

            switch(parameters.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p=>p.Name);
                    break;

                case ProductSortingOptions.NameDesc:
                    AddOrderBy(p=>p.Name);
                    break;

                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p=>p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    AddOrderBy(p=>p.Price);
                    break;

                default:
                    break;

            }

            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
        public ProductWithBrandAndTypeSpecifications(int id):base(p=>p.Id==id) 
        {
            AddIncludes(p => p.ProductBrand);
            AddIncludes(p => p.ProductType);
        }
    }
}
