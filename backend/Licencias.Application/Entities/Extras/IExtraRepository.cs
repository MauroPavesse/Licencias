using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Extras
{
    public interface IExtraRepository : IBaseRepository<Extra>
    {
        public Task<IEnumerable<Extra>> SearchAsync(
            Expression<Func<Extra, bool>>? predicate = null,
            IEnumerable<string>? includesString = null,
            bool disableTracking = true);
    }
}
