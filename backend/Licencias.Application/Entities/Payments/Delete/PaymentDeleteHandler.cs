using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.Payments.Delete
{
    public record PaymentDeleteCommand(int Id) : IRequest<bool>;

    public class PaymentDeleteHandler : IRequestHandler<PaymentDeleteCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public PaymentDeleteHandler(IPaymentRepository paymentRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _paymentRepository = paymentRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(PaymentDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(request.Id);
            if (existingPayment == null)
            {
                throw new KeyNotFoundException($"Payment with Id {request.Id} not found.");
            }
            var result = await _paymentRepository.DeleteAsync(existingPayment);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
