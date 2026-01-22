using Licencias.Application.Entities.Payments.DTOs;
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

        public PaymentCreateHandler(IPaymentRepository paymentRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<PaymentOutput> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.CreateAsync(request.Adapt<Payment>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return payment.Adapt<PaymentOutput>();
        }
    }
}
