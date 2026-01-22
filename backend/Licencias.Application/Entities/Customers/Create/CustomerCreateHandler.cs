using Licencias.Application.Entities.Customers.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Customers.Create
{
    public record CustomerCreateCommand(string Name, string Email, string PhoneNumber, string Business) : IRequest<CustomerOutput>;

    public class CustomerCreateHandler : IRequestHandler<CustomerCreateCommand, CustomerOutput>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CustomerCreateHandler(ICustomerRepository customerRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<CustomerOutput> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.CreateAsync(request.Adapt<Customer>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return customer.Adapt<CustomerOutput>();
        }
    }
}
