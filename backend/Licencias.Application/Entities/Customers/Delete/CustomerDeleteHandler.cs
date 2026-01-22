using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.Customers.Delete
{
    public record CustomerDeleteCommand(int Id) : IRequest<bool>;

    public class CustomerDeleteHandler : IRequestHandler<CustomerDeleteCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public CustomerDeleteHandler(ICustomerRepository customerRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Custoemr with Id {request.Id} not found.");
            }
            var result = await _customerRepository.DeleteAsync(existingCustomer);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
