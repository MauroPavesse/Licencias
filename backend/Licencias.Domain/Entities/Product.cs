using Licencias.Domain.Common;

namespace Licencias.Domain.Entities
{
    public class Product : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();
    }
}
