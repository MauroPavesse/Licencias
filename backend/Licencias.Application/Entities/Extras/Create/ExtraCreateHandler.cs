using Licencias.Application.Entities.Extras.DTOs;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Entities;
using Mapster;
using MediatR;

namespace Licencias.Application.Entities.Extras.Create
{
    public record ExtraCreateCommand(string Name, string Description, decimal Price) : IRequest<ExtraOutput>;

    public class ExtraCreateHandler : IRequestHandler<ExtraCreateCommand, ExtraOutput>
    {
        private readonly IExtraRepository _extraRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        public ExtraCreateHandler(IExtraRepository extraRepository, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _extraRepository = extraRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ExtraOutput> Handle(ExtraCreateCommand request, CancellationToken cancellationToken)
        {
            var extra = await _extraRepository.CreateAsync(request.Adapt<Extra>());
            await _unitOfWorkRepository.SaveChangesAsync();
            return extra.Adapt<ExtraOutput>();
        }
    }
}
