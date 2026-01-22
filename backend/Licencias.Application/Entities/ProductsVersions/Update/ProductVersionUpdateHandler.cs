using Licencias.Application.Entities.ProductsVersions.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.ProductsVersions.Update
{
    public record ProductVersionUpdateCommand(int Id, string Name, string Description, decimal Price, int ProductId) : IRequest<ProductVersionOutput>;

    public class ProductVersionUpdateHandler : IRequestHandler<ProductVersionUpdateCommand, ProductVersionOutput>
    {
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductVersionUpdateHandler(IProductVersionRepository productVersionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productVersionRepository = productVersionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ProductVersionOutput> Handle(ProductVersionUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingProductVersion = await _productVersionRepository.GetByIdAsync(request.Id);
            if (existingProductVersion == null)
            {
                throw new KeyNotFoundException($"ProductVersion with Id {request.Id} not found.");
            }
            existingProductVersion.Name = request.Name;
            existingProductVersion.Description = request.Description;
            existingProductVersion.Price = request.Price;
            existingProductVersion.ProductId = request.ProductId;
            var updatedProductVersion = await _productVersionRepository.UpdateAsync(existingProductVersion);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedProductVersion.Adapt<ProductVersionOutput>();
        }
    }
}
