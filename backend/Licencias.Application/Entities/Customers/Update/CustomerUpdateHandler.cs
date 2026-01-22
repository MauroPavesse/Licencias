using Licencias.Application.Entities.Customers.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Customers.Update
{
    public record CustomerUpdateCommand(int Id, string Name, string Email, string PhoneNumber, string Business) : IRequest<CustomerOutput>;

    public class CustomerUpdateHandler : IRequestHandler<CustomerUpdateCommand, CustomerOutput>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CustomerUpdateHandler(ICustomerRepository customerRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<CustomerOutput> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with Id {request.Id} not found.");
            }
            existingCustomer.Name = request.Name;
            existingCustomer.Email = request.Email;
            existingCustomer.PhoneNumber = request.PhoneNumber;
            existingCustomer.Business = request.Business;
            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedCustomer.Adapt<CustomerOutput>();
        }
    }
}
