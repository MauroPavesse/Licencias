using Licencias.Application.Entities.Extras.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Extras.Update
{
    public record ExtraUpdateCommand(int Id, string Name, string Description, decimal Price) : IRequest<ExtraOutput>;

    public class ExtraUpdateHandler : IRequestHandler<ExtraUpdateCommand, ExtraOutput>
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ExtraUpdateHandler(IExtraRepository extraRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _extraRepository = extraRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ExtraOutput> Handle(ExtraUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingExtra = await _extraRepository.GetByIdAsync(request.Id);
            if (existingExtra == null)
            {
                throw new KeyNotFoundException($"Extra with Id {request.Id} not found.");
            }
            existingExtra.Name = request.Name;
            existingExtra.Description = request.Description;
            existingExtra.Price = request.Price;
            var updatedExtra = await _extraRepository.UpdateAsync(existingExtra);
            await _unitOfWorkRepository.SaveChangesAsync();
            return updatedExtra.Adapt<ExtraOutput>();
        }
    }
}
