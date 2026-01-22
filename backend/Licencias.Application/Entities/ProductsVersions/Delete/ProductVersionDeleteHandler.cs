using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.ProductsVersions.Delete
{
    public record ProductVersionDeleteCommand(int Id) : IRequest<bool>;

    public class ProductVersionDeleteHandler : IRequestHandler<ProductVersionDeleteCommand, bool>
    {
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductVersionDeleteHandler(IProductVersionRepository productVersionRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productVersionRepository = productVersionRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(ProductVersionDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingProductVersion = await _productVersionRepository.GetByIdAsync(request.Id);
            if (existingProductVersion == null)
            {
                throw new KeyNotFoundException($"ProductVersion with Id {request.Id} not found.");
            }
            var result = await _productVersionRepository.DeleteAsync(existingProductVersion);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
