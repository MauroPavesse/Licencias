using Licencias.Application.Entities.Customers;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> SearchAsync(
            Expression<Func<Customer, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<Customer>, IQueryable<Customer>>> includes = [];

            if (includesString != null && includesString.Any())
            {
                foreach (var include in includesString)
                {
                    switch (include)
                    {
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
                    .Include(t => t.Subscriptions));
            }

            return await GetAllAsync(predicate, includes, disableTracking);
        }
    }
}
