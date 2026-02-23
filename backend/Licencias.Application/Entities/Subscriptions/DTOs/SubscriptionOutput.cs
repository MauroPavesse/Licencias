using Licencias.Application.Entities.Customers.DTOs;
using Licencias.Application.Entities.Extras.DTOs;
using Licencias.Application.Entities.Payments.DTOs;
using Licencias.Application.Entities.ProductsVersions.DTOs;
using Licencias.Domain.Enums;

namespace Licencias.Application.Entities.Subscriptions.DTOs
{
    public class SubscriptionOutput
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public StateEnum State { get; set; }
        public string StateName => State.ToString();
        public int CustomerId { get; set; }
        public CustomerOutput? Customer { get; set; }
        public int ProductVersionId { get; set; }
        public ProductVersionOutput? ProductVersion { get; set; }
        public IEnumerable<PaymentOutput> Payments { get; set; } = new List<PaymentOutput>();
        public IEnumerable<ExtraOutput> Extras { get; set; } = new List<ExtraOutput>();
        public decimal AmountTotal => ProductVersion != null ? Extras.Sum(t => t.Price) + ProductVersion.Price : Extras != null ? Extras.Sum(t => t.Price) : 0;
        public string Token { get; set; } = string.Empty;
        public string HardwareId { get; set; } = string.Empty;
    }
}
