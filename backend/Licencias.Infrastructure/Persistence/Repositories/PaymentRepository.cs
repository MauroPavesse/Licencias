using Licencias.Application.Entities.Payments;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> SearchAsync(
            Expression<Func<Payment, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<Payment>, IQueryable<Payment>>> includes = [];

            if (includesString != null && includesString.Any())
            {
                foreach (var include in includesString)
                {
                    switch (include)
                    {
                        case "Subscription":
                            includes.Add(i => i
                                .Include(t => t.Subscription));
                            break;
                    }
                }
            }
            else
            {
                includes.Add(i => i
                    .Include(t => t.Subscription));
            }

            return await GetAllAsync(predicate, includes, disableTracking);
        }
    }
}
