using Licencias.Application.Entities.Extras;
using Licencias.Application.Entities.Subscriptions.CreateToken;
using Licencias.Application.Entities.Subscriptions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Licencias.Domain.Enums;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.Create
{
    public record SubscriptionCreateCommand(DateTime StartDate, DateTime ExpirationDate, StateEnum State, int CustomerId, int ProductVersionId, List<int> ExtrasIds, string HardwareId) : IRequest<SubscriptionOutput>;

    public class SubscriptionCreateHandler : IRequestHandler<SubscriptionCreateCommand, SubscriptionOutput>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly SubscriptionCreateTokenService subscriptionCreateTokenService;
        private readonly IExtraRepository _extraRepository;

        public SubscriptionCreateHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWorkRepository unitOfWorkRepository, SubscriptionCreateTokenService subscriptionCreateTokenService, IExtraRepository extraRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            this.subscriptionCreateTokenService = subscriptionCreateTokenService;
            _extraRepository = extraRepository;
        }

        public async Task<SubscriptionOutput> Handle(SubscriptionCreateCommand request, CancellationToken cancellationToken)
        {
            var subscription = new Subscription
            {
                StartDate = request.StartDate,
                ExpirationDate = request.ExpirationDate,
                State = request.State,
                CustomerId = request.CustomerId,
                ProductVersionId = request.ProductVersionId,
                HardwareId = request.HardwareId,
                Token = subscriptionCreateTokenService.GenerarToken(request.CustomerId, request.ExpirationDate, request.HardwareId, request.ProductVersionId, (int)request.State)
            };

            if (request.ExtrasIds != null && request.ExtrasIds.Any())
            {
                foreach (var id in request.ExtrasIds)
                {
                    var extraStub = new Extra { Id = id };

                    var existingExtra = await _extraRepository.GetByIdAsync(id);

                    if (existingExtra != null)
                    {
                        subscription.Extras.Add(existingExtra);
                    }
                }
            }

            await _subscriptionRepository.CreateAsync(subscription);
            await _unitOfWorkRepository.SaveChangesAsync();

            return subscription.Adapt<SubscriptionOutput>();
        }
    }
}
