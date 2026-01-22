using Licencias.Application.Entities.UnitOfWork;
using MediatR;

namespace Licencias.Application.Entities.Extras.Delete
{
    public record ExtraDeleteCommand(int Id) : IRequest<bool>;

    public class ExtraDeleteHandler : IRequestHandler<ExtraDeleteCommand, bool>
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ExtraDeleteHandler(IExtraRepository extraRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _extraRepository = extraRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> Handle(ExtraDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingExtra = await _extraRepository.GetByIdAsync(request.Id);
            if (existingExtra == null)
            {
                throw new KeyNotFoundException($"Extra with Id {request.Id} not found.");
            }
            var result = await _extraRepository.DeleteAsync(existingExtra);
            await _unitOfWorkRepository.SaveChangesAsync();
            return result;
        }
    }
}
