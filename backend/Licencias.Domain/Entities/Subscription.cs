using Licencias.Domain.Common;
using Licencias.Domain.Enums;

namespace Licencias.Domain.Entities
{
    public class Subscription : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public StateEnum State { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int ProductVersionId { get; set; }
        public ProductVersion? ProductVersion { get; set; }
        public string Token { get; set; } = string.Empty;
        public string HardwareId { get; set; } = string.Empty;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Extra> Extras { get; set; } = new List<Extra>();
    }
}
