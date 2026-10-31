
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Licencias.Application.Entities.Subscriptions.CreateToken
{
    public class RsaTokenService
    {
        private readonly string _privateKeyPem;

        public RsaTokenService(IConfiguration configuration)
        {
            _privateKeyPem = configuration["LicenseSettings:PrivateKey"]
                ?.Replace("\\n", "\n")
                .Replace("\r", "")
                .Trim()
                ?? throw new InvalidOperationException(
                    "La clave privada no está configurada.");
        }

        public string GenerarToken(
            int customerId,
            DateTime expiracion,
            string hardwareId,
            int productVersionId)
        {
            string payload =
                $"{customerId}|{expiracion:yyyy-MM-dd}|{hardwareId}|{productVersionId}";

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            using RSA rsa = RSA.Create();

            rsa.ImportFromPem(_privateKeyPem);

            byte[] signatureBytes = rsa.SignData(
                payloadBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            string payloadBase64 = Convert.ToBase64String(payloadBytes);
            string signatureBase64 = Convert.ToBase64String(signatureBytes);

            return $"{payloadBase64}.{signatureBase64}";
        }
    }
}
