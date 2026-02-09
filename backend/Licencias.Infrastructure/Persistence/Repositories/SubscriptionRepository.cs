using Licencias.Application.Entities.Subscriptions;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Subscription>> SearchAsync(
            Expression<Func<Subscription, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<Subscription>, IQueryable<Subscription>>> includes = [];

            if (includesString != null && includesString.Any())
            {
                foreach (var include in includesString)
                {
                    switch (include)
                    {
                        case "Customer":
                            includes.Add(i => i
                                .Include(t => t.Customer));
                            break;

                        case "ProductVersion":
                            includes.Add(i => i
                                .Include(t => t.ProductVersion));
                            break;

                        case "Payments":
                            includes.Add(i => i
                                .Include(t => t.Payments));
                            break;

                        case "Extras":
                            includes.Add(i => i
                                .Include(t => t.Extras));
                            break;
                    }
                }
            }
            else
            {
                includes.Add(i => i
                    .Include(t => t.Customer)
                    .Include(t => t.ProductVersion!.Product)
                    .Include(t => t.Payments)
                    .Include(t => t.Extras));
            }

            return await GetAllAsync(predicate, includes, disableTracking);
        }
    }
}
