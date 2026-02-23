using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Subscriptions.GetSubscription
{
    public record SubscriptionGetSubscriptionCommand(int CustomerId, int ProductVersionId, string HardwareId) : IRequest<List<GetSubscriptionOutput>>;

    public class SubscriptionGetSubscriptionHandler : IRequestHandler<SubscriptionGetSubscriptionCommand, List<GetSubscriptionOutput> >
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionGetSubscriptionHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<GetSubscriptionOutput>> Handle(SubscriptionGetSubscriptionCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Subscription, bool>> predicate = PredicateBuilder.True<Subscription>();

            predicate = predicate.And(t => request.CustomerId == t.CustomerId && request.HardwareId == t.HardwareId && request.ProductVersionId == t.ProductVersionId);

            var subscriptions = await _subscriptionRepository.SearchAsync(predicate);

            var outputs = subscriptions.Adapt<List<GetSubscriptionOutput>>();
            return outputs;
        }
    }
}
