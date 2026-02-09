namespace Licencias.Application.Entities.Customers.DTOs
{
    public class CustomerOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Business { get; set; } = string.Empty;
    }
}
