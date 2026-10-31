using Licencias.Application.Entities.Subscriptions.CreateToken;
using Licencias.Application.Entities.UnitOfWork;
using Licencias.Domain.Enums;
using MediatR;

namespace Licencias.Application.Entities.Subscriptions.ValidateLicenseQuery
{
    public record ValidateLicenseQuery(string LicenseKey, string HardwareId) : IRequest<LicenseValidationResponse>;

    public record LicenseValidationResponse(bool IsValid, string Token, string Message);

    public class ValidateLicenseQueryHandler : IRequestHandler<ValidateLicenseQuery, LicenseValidationResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly RsaTokenService _tokenService;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ValidateLicenseQueryHandler(ISubscriptionRepository subscriptionRepository, RsaTokenService tokenService, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _tokenService = tokenService;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<LicenseValidationResponse> Handle(ValidateLicenseQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.LicenseKey) || string.IsNullOrWhiteSpace(request.HardwareId))
            {
                return new LicenseValidationResponse(false, string.Empty, "Datos incompletos.");
            }

            // 1. Buscamos la suscripción por su clave (asumiendo que tenés un campo LicenseKey o similar en Subscription)
            var subscription = (await _subscriptionRepository.SearchAsync(s => s.LicenseKey.ToLower() == request.LicenseKey.ToLower())).OrderByDescending(x => x.Id).FirstOrDefault();

            if (subscription == null)
            {
                return new LicenseValidationResponse(false, string.Empty, "La clave de licencia no existe.");
            }

            // 2. Validación de Hardware (Vinculación)
            if (string.IsNullOrEmpty(subscription.HardwareId))
            {
                // Primer uso: Vinculamos esta PC a la suscripción
                subscription.HardwareId = request.HardwareId;
                await _subscriptionRepository.UpdateAsync(subscription);
                await _unitOfWorkRepository.SaveChangesAsync();
            }
            else if (subscription.HardwareId.ToLower() != request.HardwareId.ToLower())
            {
                return new LicenseValidationResponse(false, string.Empty, "La licencia ya está registrada en otra computadora.");
            }

            // 3. Validación de Fechas y Estado (Zona horaria Argentina)
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo argTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
            DateTime fechaArgentina = TimeZoneInfo.ConvertTimeFromUtc(utcNow, argTimeZone);

            bool isValidState = subscription.State == StateEnum.Activo &&
                                subscription.ExpirationDate > fechaArgentina;

            if (!isValidState)
            {
                return new LicenseValidationResponse(false, string.Empty, "La suscripción está vencida o inactiva.");
            }

            // 4. Generamos el token usando tu servicio existente
            string token = _tokenService.GenerarToken(
                subscription.CustomerId,
                subscription.ExpirationDate,
                subscription.HardwareId,
                subscription.ProductVersionId
            );

            return new LicenseValidationResponse(true, token, "Licencia validada con éxito.");
        }
    }
}
