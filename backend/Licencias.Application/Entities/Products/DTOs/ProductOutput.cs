using Licencias.Application.Entities.ProductsVersions.DTOs;

namespace Licencias.Application.Entities.Products.DTOs
{
    public class ProductOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProductVersionOutput> ProductVersions { get; set; } = new List<ProductVersionOutput>();
    }
}
