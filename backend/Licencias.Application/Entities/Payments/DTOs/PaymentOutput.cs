using Licencias.Application.Entities.Subscriptions.DTOs;

namespace Licencias.Application.Entities.Payments.DTOs
{
    public class PaymentOutput
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime Period { get; set; }
        public int SubscriptionId { get; set; }
        public SubscriptionOutput? Subscription { get; set; }
    }
}
