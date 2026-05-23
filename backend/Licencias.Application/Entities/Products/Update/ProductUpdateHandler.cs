using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Entities.ProductsVersions.Update;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Products.Update
{
    public record ProductUpdateCommand(int Id, string Name, string Description, List<ProductVersionUpdateCommand> ProductVersions) : IRequest<ProductOutput>;

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
            var existingProduct = (await _productRepository.SearchAsync(p => p.Id == request.Id, ["ProductVersions"], false)).FirstOrDefault();
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found.");
            }
            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;

            SynchronizeVersions(existingProduct, request.ProductVersions);

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            await _unitOfWorkRepository.SaveChangesAsync();
            
            return updatedProduct.Adapt<ProductOutput>();
        }

        private void SynchronizeVersions(Product product, List<ProductVersionUpdateCommand> requestProductVersions)
        {
            requestProductVersions ??= new List<ProductVersionUpdateCommand>();

            var productVersionsToRemove = product.ProductVersions
                .Where(pvDb => !requestProductVersions.Any(pvReq => pvReq.Id == pvDb.Id))
                .ToList();

            foreach (var productVersion in productVersionsToRemove)
            {
                product.ProductVersions.Remove(productVersion);
            }

            foreach (var reqVersion in requestProductVersions)
            {
                var existingVersion = product.ProductVersions.FirstOrDefault(pvDb => pvDb.Id == reqVersion.Id && pvDb.Id != 0);

                if (existingVersion != null)
                {
                    existingVersion.Name = reqVersion.Name;
                    existingVersion.Description = reqVersion.Description;
                    existingVersion.Price = reqVersion.Price;
                }
                else
                {
                    var newVersion = reqVersion.Adapt<ProductVersion>();
                    newVersion.Id = 0;

                    product.ProductVersions.Add(newVersion);
                }
            }
        }
    }
}
