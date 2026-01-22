using FluentValidation;
using Licencias.Domain.Entities;

namespace Licencias.Application.Entities.Products
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");
            
            RuleFor(t => t.Description)
                .MaximumLength(300).WithMessage("La descripción no puede superar los 300 caracteres");
        }
    }
}
