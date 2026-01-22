using Licencias.Application.Entities.Extras.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Extras.Search
{
    public record ExtraSearchCommand(SearchCommand Search) : IRequest<IEnumerable<ExtraOutput>>;

    public class ExtraSearchHandler : IRequestHandler<ExtraSearchCommand, IEnumerable<ExtraOutput>>
    {
        private readonly IExtraRepository _extraRepository;

        public ExtraSearchHandler(IExtraRepository extraRepository)
        {
            _extraRepository = extraRepository;
        }

        public async Task<IEnumerable<ExtraOutput>> Handle(ExtraSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<Extra, bool>> predicate = PredicateBuilder.True<Extra>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var extrasIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "ExtrasIds");
                if (extrasIdsFilter != null)
                {
                    predicate = predicate.And(t => extrasIdsFilter.Ids.Contains(t.Id));
                }
            }

            var extras = await _extraRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            return extras.Adapt<IEnumerable<ExtraOutput>>();
        }
    }
}
