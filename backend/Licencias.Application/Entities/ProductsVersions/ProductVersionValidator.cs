using FluentValidation;
using Licencias.Domain.Entities;

namespace Licencias.Application.Entities.ProductsVersions
{
    public class ProductVersionValidator : AbstractValidator<ProductVersion>
    {
        public ProductVersionValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");

            RuleFor(t => t.Description)
                .MaximumLength(300).WithMessage("La descripcion no puede superar los 300 caracteres");
        }
    }
}
