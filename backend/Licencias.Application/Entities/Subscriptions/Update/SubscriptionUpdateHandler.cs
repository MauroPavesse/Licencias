using Licencias.Application.Entities.Extras;
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
    public record SubscriptionUpdateCommand(int Id, DateTime StartDate, DateTime ExpirationDate, StateEnum State, int CustomerId, int ProductVersionId, List<int> ExtrasIds, string HardwareId) : IRequest<SubscriptionOutput>;

    public class SubscriptionUpdateHandler : IRequestHandler<SubscriptionUpdateCommand, SubscriptionOutput>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly SubscriptionCreateTokenService subscriptionCreateTokenService;
        private readonly IExtraRepository _extraRepository;

        public SubscriptionUpdateHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository, SubscriptionCreateTokenService subscriptionCreateTokenService, IExtraRepository extraRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            this.subscriptionCreateTokenService = subscriptionCreateTokenService;
            _extraRepository = extraRepository;
        }

        public async Task<SubscriptionOutput> Handle(SubscriptionUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingSubscription = (await _subscriptionRepository.SearchAsync(s => s.Id == request.Id, ["Extras"], false)).FirstOrDefault();
            if (existingSubscription == null)
            {
                throw new KeyNotFoundException($"Subscription with Id {request.Id} not found.");
            }

            // 1. Actualizar datos primitivos
            existingSubscription.StartDate = request.StartDate;
            existingSubscription.ExpirationDate = request.ExpirationDate;
            existingSubscription.State = request.State;
            existingSubscription.CustomerId = request.CustomerId;
            existingSubscription.ProductVersionId = request.ProductVersionId;
            existingSubscription.HardwareId = request.HardwareId;

            existingSubscription.Token = subscriptionCreateTokenService.GenerarToken(
                existingSubscription.CustomerId,
                existingSubscription.ExpirationDate,
                existingSubscription.HardwareId,
                existingSubscription.ProductVersionId,
                (int)existingSubscription.State
            );

            // 2. Sincronizar la colección de Extras
            await SynchronizeExtrasAsync(existingSubscription, request.ExtrasIds);

            var updatedSubscription = await _subscriptionRepository.UpdateAsync(existingSubscription);
            await _unitOfWorkRepository.SaveChangesAsync();

            return updatedSubscription.Adapt<SubscriptionOutput>();
        }

        private async Task SynchronizeExtrasAsync(Subscription subscription, List<int> requestExtraIds)
        {
            // Inicializar la lista si por alguna razón llega null en el request
            requestExtraIds ??= new List<int>();

            // 1. Remover los extras que ya no se quieren
            var extrasToRemove = subscription.Extras
                .Where(e => !requestExtraIds.Contains(e.Id))
                .ToList();

            foreach (var extra in extrasToRemove)
            {
                subscription.Extras.Remove(extra);
            }

            // 2. Agregar los nuevos extras que no estaban previamente
            var currentExtraIds = subscription.Extras.Select(e => e.Id).ToList();
            var extrasToAddIds = requestExtraIds.Where(id => !currentExtraIds.Contains(id)).ToList();

            foreach (var id in extrasToAddIds)
            {
                var existingExtra = await _extraRepository.GetByIdAsync(id);
                if (existingExtra != null)
                {
                    subscription.Extras.Add(existingExtra);
                }
            }
        }
    }
}
