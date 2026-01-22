using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Payments
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        public Task<IEnumerable<Payment>> SearchAsync(
            Expression<Func<Payment, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
