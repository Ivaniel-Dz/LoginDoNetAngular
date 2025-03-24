using Microsoft.IdentityModel.Tokens; // Para manejar la creación de credenciales de seguridad y claves
using System.IdentityModel.Tokens.Jwt; // Para generar y manejar tokens JWT
using System.Security.Claims; // Para manejar los claims (información del usuario) dentro del token
using System.Security.Cryptography; // Para manejar el algoritmo de cifrado SHA256
using System.Text; // Para convertir cadenas a bytes
using LoginAPI.Models; // Referencia al modelo Usuario

namespace LoginAPI.Jwt
{
    public class JwtUtil
    {
        private readonly IConfiguration _configuration; // Inyección de dependencia para acceder a la configuración de la aplicación
        public JwtUtil(IConfiguration configuration)
        {
            _configuration = configuration; // Asignación de la configuración inyectada
        }

        // Método para encriptar una cadena de texto utilizando SHA256
        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create()) // Crear una instancia de SHA256
            {
                // Computar el hash a partir del texto
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) // Convertir el resultado a formato hexadecimal
                {
                    builder.Append(bytes[i].ToString("x2")); // Formatear el valor como hexadecimal
                }
                return builder.ToString(); // Devolver la cadena encriptada
            }
        }

        // Método para generar un token JWT a partir del modelo de usuario
        public string generarJWT(Usuario modelo)
        {
            // Crear los claims (información del usuario) que irán dentro del token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.Id.ToString()), // Claim con el ID del usuario
                new Claim(ClaimTypes.Email, modelo.Correo!) // Claim con el correo del usuario
            };

            // Obtener la clave de firma simétrica desde la configuración
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            // Crear credenciales de firma utilizando la clave y el algoritmo HMAC SHA256
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Detalles del token (claims, tiempo de expiración y firma)
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims, // Claims del token
                expires: DateTime.UtcNow.AddMinutes(10), // Expira en 10 minutos
                signingCredentials: credential // Credenciales de firma
                );

            // Crear y devolver el token JWT como cadena
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public bool validarToken(string token)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // Validar la clave de firma del emisor
                ValidateIssuer = false, // No validar el emisor (puedes cambiarlo según sea necesario)
                ValidateAudience = false, // No validar la audiencia (puedes cambiarlo según sea necesario)
                ValidateLifetime = true, // Validar que el token no esté expirado
                ClockSkew = TimeSpan.Zero, // No permitir margen de tiempo adicional para expiración
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!)) // Clave de firma del token
            };

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch(SecurityTokenExpiredException) {
                return false;
            }
            catch(SecurityTokenInvalidSignatureException) {
                return false;
            }
            catch (Exception ex) {
                return false;
            }
        }

    }
}
