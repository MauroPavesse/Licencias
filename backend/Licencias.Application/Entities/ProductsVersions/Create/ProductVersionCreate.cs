using Licencias.Application.Entities.ProductsVersions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.ProductsVersions.Create
{
    public record ProductVersionCreateCommand(string Name, string Description, decimal Price, int ProductId) : IRequest<ProductVersionOutput>;

    public class ProductVersionCreate : IRequestHandler<ProductVersionCreateCommand, ProductVersionOutput>
    {
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductVersionCreate(IProductVersionRepository productVersionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productVersionRepository = productVersionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ProductVersionOutput> Handle(ProductVersionCreateCommand request, CancellationToken cancellationToken)
        {
            var productVersion = await _productVersionRepository.CreateAsync(request.Adapt<ProductVersion>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return productVersion.Adapt<ProductVersionOutput>();
        }
    }
}
