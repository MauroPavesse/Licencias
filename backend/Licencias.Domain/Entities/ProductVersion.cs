using Licencias.Domain.Common;

namespace Licencias.Domain.Entities
{
    public class ProductVersion : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();

        public IEnumerable<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
