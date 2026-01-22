using FluentValidation;
using Licencias.Domain.Entities;

namespace Licencias.Application.Entities.Payments
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(t => t.PaymentDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de pago no puede ser en el futuro");
        }
    }
}
