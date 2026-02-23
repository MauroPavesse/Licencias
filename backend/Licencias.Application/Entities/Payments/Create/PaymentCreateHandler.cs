using Licencias.Application.Entities.Payments.DTOs;
using Licencias.Application.Entities.Subscriptions;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Payments.Create
{
    public record PaymentCreateCommand(decimal Amount, DateTime PaymentDate, DateTime Period, int SubscriptionId) : IRequest<PaymentOutput>;

    public class PaymentCreateHandler : IRequestHandler<PaymentCreateCommand, PaymentOutput>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PaymentCreateHandler(IPaymentRepository paymentRepository, IUnitOfWorkRepository unitOfWorkRepository, ISubscriptionRepository subscriptionRepository)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<PaymentOutput> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
        {
            var payment = request.Adapt<Payment>();
            var createdPayment = await _paymentRepository.CreateAsync(payment);

            var subscription = await _subscriptionRepository.GetByIdAsync(createdPayment.SubscriptionId);
            if(subscription != null)
            {
                subscription.ExpirationDate = subscription.ExpirationDate.AddMonths(1);
                await _subscriptionRepository.UpdateAsync(subscription);
            }

            await _unitOfWorkRepository.SaveChangesAsync();
            return createdPayment.Adapt<PaymentOutput>();
        }
    }
}
