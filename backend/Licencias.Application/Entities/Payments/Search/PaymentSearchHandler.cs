using Licencias.Application.Entities.Payments.DTOs;
using Licencias.Application.Shared;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace Licencias.Application.Entities.Payments.Search
{
    public record PaymentSearchCommand(SearchCommand Search) : IRequest<IEnumerable<PaymentOutput>>;

    public class PaymentSearchHandler : IRequestHandler<PaymentSearchCommand, IEnumerable<PaymentOutput>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentSearchHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentOutput>> Handle(PaymentSearchCommand request, CancellationToken cancellationToken)
        {
            var search = request.Search;

            Expression<Func<Payment, bool>> predicate = PredicateBuilder.True<Payment>();

            if (search.Id > 0)
            {
                predicate = t => t.Id == search.Id;
            }
            else
            {
                var paymentsIdsFilter = search.Filters?.FirstOrDefault(x => x.Field == "PaymentsIds");
                if (paymentsIdsFilter != null)
                {
                    predicate = predicate.And(t => paymentsIdsFilter.Ids.Contains(t.Id));
                }
            }

            var products = await _paymentRepository.SearchAsync(predicate, search.Includes, search.DisableTracking);

            return products.Adapt<IEnumerable<PaymentOutput>>();
        }
    }
}
