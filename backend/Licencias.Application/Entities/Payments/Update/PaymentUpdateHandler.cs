using Licencias.Application.Entities.Payments.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Payments.Update
{
    public record PaymentUpdateCommand(int Id, decimal Amount, DateTime PaymentDate, DateTime Period, int SubscriptionId) : IRequest<PaymentOutput>;

    public class PaymentUpdateHandler : IRequestHandler<PaymentUpdateCommand, PaymentOutput>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public PaymentUpdateHandler(IPaymentRepository paymentRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<PaymentOutput> Handle(PaymentUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(request.Id);
            if (existingPayment == null)
            {
                throw new KeyNotFoundException($"Payment with Id {request.Id} not found.");
            }
            existingPayment.Amount = request.Amount;
            existingPayment.PaymentDate = request.PaymentDate;
            existingPayment.Period = request.Period;
            existingPayment.SubscriptionId = request.SubscriptionId;
            var updatedPayment = await _paymentRepository.UpdateAsync(existingPayment);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedPayment.Adapt<PaymentOutput>();
        }
    }
}
