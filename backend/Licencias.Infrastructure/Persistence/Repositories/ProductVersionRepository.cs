using Licencias.Application.Entities.ProductsVersions;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class ProductVersionRepository : BaseRepository<ProductVersion>, IProductVersionRepository
    {
        public ProductVersionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductVersion>> SearchAsync(
            Expression<Func<ProductVersion, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<ProductVersion>, IQueryable<ProductVersion>>> includes = [];

            if (includesString != null && includesString.Any())
            {
                foreach (var include in includesString)
                {
                    switch (include)
                    {
                        case "Product":
                            includes.Add(i => i
                                .Include(t => t.Product));
                            break;

                        case "Subscriptions":
                            includes.Add(i => i
                                .Include(t => t.Subscriptions));
                            break;
                    }
                }
            }
            else
            {
                includes.Add(i => i
                    .Include(t => t.Product)
                    .Include(t => t.Subscriptions));
            }

            return await GetAllAsync(predicate, includes, disableTracking);
        }
    }
}
