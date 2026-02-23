using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Entities.Subscriptions.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Subscriptions.Search
{
    public record SubscriptionSearchCommand(SearchCommand Search) : IRequest<IEnumerable<SubscriptionOutput>>;

    public class SubscriptionSearchHandler : IRequestHandler<SubscriptionSearchCommand, IEnumerable<SubscriptionOutput>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionSearchHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IEnumerable<SubscriptionOutput>> Handle(SubscriptionSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<Subscription, bool>> predicate = PredicateBuilder.True<Subscription>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var subscriptionsIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "SubscriptionsIds");
                if (subscriptionsIdsFilter != null)
                {
                    predicate = predicate.And(t => subscriptionsIdsFilter.Ids.Contains(t.Id));
                }
            }

            var subscriptions = await _subscriptionRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            var outputs = subscriptions.Adapt<List<SubscriptionOutput>>();
            return outputs;
        }
    }
}
