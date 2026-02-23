namespace Licencias.Application.Entities.Subscriptions.GetSubscription
{
    public class GetSubscriptionOutput
    {
        public int CustomerId { get; set; } 
        public int ProductVersionId { get; set; } 
        public string HardwareId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
