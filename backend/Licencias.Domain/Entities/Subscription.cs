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
        public Customer Customer { get; set; } = new ();
        public int ProductVersionId { get; set; }
        public ProductVersion ProductVersion { get; set; } = new ();

        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public IEnumerable<Extra> Extras { get; set; } = new List<Extra>();
    }
}
