using Licencias.Domain.Common;

namespace Licencias.Domain.Entities
{
    public class Payment : BaseModel
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime Period { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; } = null;
    }
}
