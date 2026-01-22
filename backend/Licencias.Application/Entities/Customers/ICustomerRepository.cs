using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Customers
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        public Task<IEnumerable<Customer>> SearchAsync(
            Expression<Func<Customer, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
