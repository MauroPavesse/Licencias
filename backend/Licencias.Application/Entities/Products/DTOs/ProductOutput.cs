using Licencias.Application.Entities.ProductsVersions.DTOs;

namespace Licencias.Application.Entities.Products.DTOs
{
    public class ProductOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<ProductVersionOutput> ProductsVersionsOutputs { get; set; } = new List<ProductVersionOutput>();
    }
}
