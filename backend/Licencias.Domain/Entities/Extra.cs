using Licencias.Domain.Common;

namespace Licencias.Domain.Entities
{
    public class Extra : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public IEnumerable<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
