using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.Delete
{
    public record SubscriptionDeleteCommand(int Id) : IRequest<bool>;

    public class SubscriptionDeleteHandler : IRequestHandler<SubscriptionDeleteCommand, bool>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        public SubscriptionDeleteHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        public async Task<bool> Handle(SubscriptionDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingSubscription = await _subscriptionRepository.GetByIdAsync(request.Id);
            if (existingSubscription == null)
            {
                throw new KeyNotFoundException($"Subscription with Id {request.Id} not found.");
            }
            var result = await _subscriptionRepository.DeleteAsync(existingSubscription);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
