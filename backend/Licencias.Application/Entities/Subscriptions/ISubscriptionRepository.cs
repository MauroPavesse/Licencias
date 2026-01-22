using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Subscriptions
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        public Task<IEnumerable<Subscription>> SearchAsync(
            Expression<Func<Subscription, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
