using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Products.Update
{
    public record ProductUpdateCommand(int Id, string Name, string Description) : IRequest<ProductOutput>;

    public class ProductUpdateHandler : IRequestHandler<ProductUpdateCommand, ProductOutput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductUpdateHandler(IProductRepository productRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productRepository = productRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ProductOutput> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found.");
            }
            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedProduct.Adapt<ProductOutput>();
        }
    }
}
