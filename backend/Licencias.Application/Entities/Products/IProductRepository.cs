using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Products
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<IEnumerable<Product>> SearchAsync(
            Expression<Func<Product, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
