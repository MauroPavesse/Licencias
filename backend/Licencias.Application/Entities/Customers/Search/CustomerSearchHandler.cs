using Licencias.Application.Entities.Customers.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Customers.Search
{
    public record CustomerSearchCommand(SearchCommand Search) : IRequest<IEnumerable<CustomerOutput>>;

    public class CustomerSearchHandler : IRequestHandler<CustomerSearchCommand, IEnumerable<CustomerOutput>>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerSearchHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerOutput>> Handle(CustomerSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<Customer, bool>> predicate = PredicateBuilder.True<Customer>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var customersIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "CustomersIds");
                if (customersIdsFilter != null)
                {
                    predicate = predicate.And(t => customersIdsFilter.Ids.Contains(t.Id));
                }
            }

            var customers = await _customerRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            return customers.Adapt<IEnumerable<CustomerOutput>>();
        }
    }
}
