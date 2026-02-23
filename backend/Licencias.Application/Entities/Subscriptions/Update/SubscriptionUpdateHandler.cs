using Licencias.Application.Entities.Extras.Create;
using Licencias.Application.Entities.Subscriptions.CreateToken;
using Licencias.Application.Entities.Subscriptions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Licencias.Domain.Enums;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.Update
{
    public record SubscriptionUpdateCommand(int Id, DateTime StartDate, DateTime ExpirationDate, StateEnum State, int CustomerId, int ProductVersionId, List<ExtraCreateCommand> Extras, string HardwareId) : IRequest<SubscriptionOutput>;

    public class SubscriptionUpdateHandler : IRequestHandler<SubscriptionUpdateCommand, SubscriptionOutput>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly SubscriptionCreateTokenService subscriptionCreateTokenService;

        public SubscriptionUpdateHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository, SubscriptionCreateTokenService subscriptionCreateTokenService)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            this.subscriptionCreateTokenService = subscriptionCreateTokenService;
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
            existingSubscription.Extras = request.Extras.Adapt<List<Extra>>();
            existingSubscription.HardwareId = request.HardwareId;

            string token = subscriptionCreateTokenService.GenerarToken(existingSubscription.CustomerId, existingSubscription.ExpirationDate, existingSubscription.HardwareId, existingSubscription.ProductVersionId, (int)existingSubscription.State);
            existingSubscription.Token = token;

            var updatedSubscription = await _subscriptionRepository.UpdateAsync(existingSubscription);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedSubscription.Adapt<SubscriptionOutput>();
        }
    }
}
