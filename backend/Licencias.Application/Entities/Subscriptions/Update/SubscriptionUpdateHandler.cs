using Licencias.Application.Entities.Subscriptions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Enums;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.Update
{
    public record SubscriptionUpdateCommand(int Id, DateTime StartDate, DateTime ExpirationDate, StateEnum State, int CustomerId, int ProductVersionId) : IRequest<SubscriptionOutput>;

    public class SubscriptionUpdateHandler : IRequestHandler<SubscriptionUpdateCommand, SubscriptionOutput>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public SubscriptionUpdateHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<SubscriptionOutput> Handle(SubscriptionUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingSubscription = await _subscriptionRepository.GetByIdAsync(request.Id);
            if (existingSubscription == null)
            {
                throw new KeyNotFoundException($"Subscription with Id {request.Id} not found.");
            }
            existingSubscription.StartDate = request.StartDate;
            existingSubscription.ExpirationDate = request.ExpirationDate;
            existingSubscription.State = request.State;
            existingSubscription.CustomerId = request.CustomerId;
            existingSubscription.ProductVersionId = request.ProductVersionId;
            var updatedSubscription = await _subscriptionRepository.UpdateAsync(existingSubscription);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedSubscription.Adapt<SubscriptionOutput>();
        }
    }
}
