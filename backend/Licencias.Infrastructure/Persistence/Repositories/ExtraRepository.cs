using Licencias.Application.Entities.Extras;
using Licencias.Domain.Entities;
using Licencias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Licencias.Infrastructure.Persistence.Repositories
{
    public class ExtraRepository : BaseRepository<Extra>, IExtraRepository
    {
        public ExtraRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Extra>> SearchAsync(
            Expression<Func<Extra, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true)
        {
            List<Func<IQueryable<Extra>, IQueryable<Extra>>> includes = [];

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
