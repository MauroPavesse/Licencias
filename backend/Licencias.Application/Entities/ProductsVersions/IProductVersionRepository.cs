using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.ProductsVersions
{
    public interface IProductVersionRepository : IBaseRepository<ProductVersion>
    {
        public Task<IEnumerable<ProductVersion>> SearchAsync(
            Expression<Func<ProductVersion, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
