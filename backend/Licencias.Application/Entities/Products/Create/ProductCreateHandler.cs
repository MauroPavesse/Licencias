using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Products.Create
{
    public record ProductCreateCommand(string Name, string Description) : IRequest<ProductOutput>;

    public class ProductCreateHandler : IRequestHandler<ProductCreateCommand, ProductOutput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ProductCreateHandler(IProductRepository productRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _productRepository = productRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ProductOutput> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.CreateAsync(request.Adapt<Product>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return product.Adapt<ProductOutput>();
        }
    }
}
