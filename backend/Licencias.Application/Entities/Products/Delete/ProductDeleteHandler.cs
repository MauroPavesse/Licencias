using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.Products.Delete
{
    public record ProductDeleteCommand(int Id) : IRequest<bool>;

    public class ProductDeleteHandler : IRequestHandler<ProductDeleteCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductDeleteHandler(IProductRepository productRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productRepository = productRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found.");
            }
            var result = await _productRepository.DeleteAsync(existingProduct);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
