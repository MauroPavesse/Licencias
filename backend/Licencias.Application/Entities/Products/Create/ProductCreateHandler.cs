using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Entities.ProductsVersions.Create;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Products.Create
{
    public record ProductCreateCommand(string Name, string Description, List<ProductVersionCreateCommand> ProductVersions) : IRequest<ProductOutput>;

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
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                // 2. Aquí está el truco: .ToList() es OBLIGATORIO para EF Core
                ProductVersions = request.ProductVersions.Select(v => new ProductVersion
                {
                    Name = v.Name, // Ojo: en tu comando es 'Name', en la entidad parece ser 'Version'
                    Description = v.Description,
                    Price = v.Price
                }).ToList()
            };

            // 3. Persistir
            var createdProduct = await _productRepository.CreateAsync(product);
            await _unitOfWorkRepository.SaveChangesAsync();

            return createdProduct.Adapt<ProductOutput>();
        }
    }
}
