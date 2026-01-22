using System.ComponentModel.DataAnnotations;

namespace Licencias.Domain.Common
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
