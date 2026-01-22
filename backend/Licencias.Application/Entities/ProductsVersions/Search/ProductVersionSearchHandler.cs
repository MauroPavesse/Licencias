using Licencias.Application.Entities.ProductsVersions.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.ProductsVersions.Search
{
    public record ProductVersionSearchCommand(SearchCommand Search) : IRequest<IEnumerable<ProductVersionOutput>>;

    public class ProductVersionSearchHandler : IRequestHandler<ProductVersionSearchCommand, IEnumerable<ProductVersionOutput>>
    {
        private readonly IProductVersionRepository _productVersionRepository;

        public ProductVersionSearchHandler(IProductVersionRepository productVersionRepository)
        {
            _productVersionRepository = productVersionRepository;
        }

        public async Task<IEnumerable<ProductVersionOutput>> Handle(ProductVersionSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<ProductVersion, bool>> predicate = PredicateBuilder.True<ProductVersion>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var productsVersionsIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "ProductsVersionsIds");
                if (productsVersionsIdsFilter != null)
                {
                    predicate = predicate.And(t => productsVersionsIdsFilter.Ids.Contains(t.Id));
                }
            }

            var productsVersions = await _productVersionRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            return productsVersions.Adapt<IEnumerable<ProductVersionOutput>>();
        }
    }
}
