namespace Licencias.Application.Entities.Extras.DTOs
{
    public class ExtraOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public IEnumerable<ExtraOutput> Extras { get; set; } = new List<ExtraOutput>();
    }
}
