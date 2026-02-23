using System.Security.Cryptography;
using System.Text;

namespace Licencias.Application.Entities.Subscriptions.CreateToken
{
    public class SubscriptionCreateTokenService
    {
        private static readonly string SecretKey = "Tu_Clave_Ultra_Secreta_998877";

        public string GenerarToken(int CustomerId, DateTime Expiracion, string HardwareId, int ProductVersionId, int stateId)
        {
            // 1. Formateamos los datos (el "payload")
            string fechaStr = Expiracion.ToString("yyyy-MM-dd");
            string data = $"{CustomerId}|{fechaStr}|{HardwareId}|{ProductVersionId}|{stateId}";

            // 2. Creamos la firma usando HMACSHA256
            string firma = CalcularHash(data, SecretKey);

            // 3. El token final es la unión de los datos + la firma
            // Convertimos los datos a Base64 para que no tengan caracteres extraños
            string dataBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));

            return $"{dataBase64}.{firma}";
        }

        private static string CalcularHash(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
