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
