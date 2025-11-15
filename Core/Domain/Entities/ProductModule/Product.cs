namespace Domain.Entities.ProductModule
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl {  get; set; } = null!; 
        public decimal Price { get; set; }
        // 1-M ProductType
        public ProductType ProductType { get; set; } // Navgational Property
        public int TypeId { get; set; }//FK
        // 1-M ProductBrand
        public ProductBrand ProductBrand { get; set; }// Navgational Property
        public int BrandId { get; set; }//FK

    }
}
