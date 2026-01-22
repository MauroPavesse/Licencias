using FluentValidation;
using Licencias.Domain.Entities;

namespace Licencias.Application.Entities.Customers
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");
            
            RuleFor(t => t.Email)
                .EmailAddress().WithMessage("El correo electrónico no es válido")
                .MaximumLength(150).WithMessage("El correo electrónico no puede superar los 150 caracteres");
            
            RuleFor(t => t.PhoneNumber)
                .MaximumLength(20).WithMessage("El número de teléfono no puede superar los 20 caracteres");
            
            RuleFor(t => t.Business)
                .MaximumLength(100).WithMessage("El negocio no puede superar los 100 caracteres");
        }
    }
}
