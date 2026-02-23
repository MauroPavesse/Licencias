using Licencias.Application.Entities.ProductsVersions.DTOs;
using Licencias.Domain.Entities;
using Mapster;

namespace Licencias.Application.Mappings
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.Default.MaxDepth(3);
        }
    }
}
