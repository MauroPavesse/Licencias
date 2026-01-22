using Licencias.Domain.Common;

namespace Licencias.Domain.Entities
{
    public class Customer : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Business { get; set; } = string.Empty;

        public IEnumerable<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
