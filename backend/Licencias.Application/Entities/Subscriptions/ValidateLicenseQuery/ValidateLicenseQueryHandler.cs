using Licencias.Domain.Entities;
using Licencias.Domain.Enums;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.ValidateLicenseQuery
{
    public record ValidateLicenseQuery(string Domain) : IRequest<LicenseValidationResponse>;

    public record LicenseValidationResponse(bool IsActive);

    public class ValidateLicenseQueryHandler
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public ValidateLicenseQueryHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<LicenseValidationResponse> Handle(ValidateLicenseQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Domain))
            {
                return new LicenseValidationResponse(false);
            }

            // Buscamos la suscripción que coincida con el dominio recibido
            var subscription = (await _subscriptionRepository.SearchAsync(s => s.HardwareId.ToLower() == request.Domain.ToLower())).FirstOrDefault();

            if (subscription == null)
            {
                // Si no existe el subdominio en el sistema de licencias, no está activo
                return new LicenseValidationResponse(false);
            }

            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo argTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
            DateTime fechaArgentina = TimeZoneInfo.ConvertTimeFromUtc(utcNow, argTimeZone);
            // Validamos: 
            // 1. Que el estado sea "Activo" (Ajustá StateEnum.Active según tu ENUM real)
            // 2. Que la fecha actual no supere la fecha de expiración
            bool isValid = subscription.State == StateEnum.Activo &&
                           subscription.ExpirationDate > fechaArgentina;

            return new LicenseValidationResponse(isValid);
        }
    }
}
