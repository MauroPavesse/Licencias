using Licencias.Application.Entities.Extras.Create;
using Licencias.Application.Entities.Subscriptions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Licencias.Domain.Enums;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.Create
{
    public record SubscriptionCreateCommand(DateTime StartDate, DateTime ExpirationDate, StateEnum State, int CustomerId, int ProductVersionId, List<ExtraCreateCommand> Extras) : IRequest<SubscriptionOutput>;

    public class SubscriptionCreateHandler : IRequestHandler<SubscriptionCreateCommand, SubscriptionOutput>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public SubscriptionCreateHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<SubscriptionOutput> Handle(SubscriptionCreateCommand request, CancellationToken cancellationToken)
        {
            var subscription = request.Adapt<Subscription>();

            await _subscriptionRepository.CreateAsync(subscription);
            await _unitOfWorkRepository.SaveChangesAsync();

            return subscription.Adapt<SubscriptionOutput>();
        }
    }
}
