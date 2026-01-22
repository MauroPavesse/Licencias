using Licencias.Application.Entities.Products;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> SearchAsync(
            Expression<Func<Product, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<Product>, IQueryable<Product>>> includes = [];

            if (includesString != null && includesString.Any())
            {
                foreach (var include in includesString)
                {
                    switch (include)
                    {
                        case "ProductVersions":
                            includes.Add(i => i
                                .Include(t => t.ProductVersions));
                            break;
                    }
                }
            }
            else
            {
                includes.Add(i => i
                    .Include(t => t.ProductVersions));
            }

            return await GetAllAsync(predicate, includes, disableTracking);
        }
    }
}
