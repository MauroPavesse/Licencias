using Licencias.Application.Entities.Products.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Products.Search
{
    public record ProductSearchCommand(SearchCommand Search) : IRequest<IEnumerable<ProductOutput>>;

    public class ProductSearchHandler : IRequestHandler<ProductSearchCommand, IEnumerable<ProductOutput>>
    {
        private readonly IProductRepository _productRepository;

        public ProductSearchHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductOutput>> Handle(ProductSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<Product, bool>> predicate = PredicateBuilder.True<Product>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var productsIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "ProductsIds");
                if (productsIdsFilter != null)
                {
                    predicate = predicate.And(t => productsIdsFilter.Ids.Contains(t.Id));
                }
            }

            var products = await _productRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            return products.Adapt<IEnumerable<ProductOutput>>();
        }
    }
}
