using Microsoft.AspNetCore.Authorization; // Para manejar la autorización
using Microsoft.AspNetCore.Mvc; // Para crear controladores y acciones en la API
using Microsoft.EntityFrameworkCore; // Para realizar consultas y operaciones de base de datos usando Entity Framework
using LoginAPI.Data; // Para utilizar la clase de utilidades personalizadas
using LoginAPI.Models; // Referencia a los modelos de la aplicación, en este caso Usuario
using LoginAPI.DTOs;
using LoginAPI.Jwt; // Referencia a los DTOs (objetos de transferencia de datos)

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")] // Definir la ruta base para el controlador (e.g. api/acceso)
    [AllowAnonymous] // Permitir acceso a este controlador sin autenticación
    [ApiController] // Indica que este controlador manejará respuestas JSON automáticamente
    public class AccesoController : ControllerBase
    {
        private readonly AppDBContext _appDBContext; // Inyección del contexto de la base de datos
        private readonly JwtUtil _jwtUtil; // Inyección de la clase de utilidades

        public AccesoController(AppDBContext appDBContext, JwtUtil jwtUtil)
        {
            _appDBContext = appDBContext; // Asignación del contexto inyectado
            _jwtUtil = jwtUtil; // Asignación de las utilidades inyectadas
        }

        // Endpoint para registrar un nuevo usuario
        [HttpPost]
        [Route("Registrarse")] // Ruta: api/acceso/registrarse
        public async Task<IActionResult> Registrarse(UsuarioDto objeto)
        {
            // Crear un nuevo modelo de usuario a partir de los datos recibidos
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _jwtUtil.encriptarSHA256(objeto.Clave),
            };

            // Agregar el nuevo usuario a la base de datos
            await _appDBContext.Usuarios.AddAsync(modeloUsuario);
            await _appDBContext.SaveChangesAsync(); // Guardar cambios en la base de datos

            // Verificar si el usuario se registró correctamente
            if (modeloUsuario.Id != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true }); // Éxito
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false }); // Error
            }
        }

        // Endpoint para el login del usuario
        [HttpPost]
        [Route("Login")] // Ruta: api/acceso/login
        public async Task<IActionResult> Login(LoginDto objeto)
        {
            // Buscar al usuario por correo y clave encriptada
            var userFound = await _appDBContext.Usuarios
                             .Where(u =>
                                u.Correo == objeto.Correo && // Verificar si el correo coincide
                                u.Clave == _jwtUtil.encriptarSHA256(objeto.Clave) // Comparar la clave encriptada
                             ).FirstOrDefaultAsync();

            // Si el usuario no es encontrado, retornar error
            if (userFound == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" }); // Usuario no encontrado
            }
            else
            {
                // Usuario encontrado, retornar éxito (token vacío por ahora)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _jwtUtil.generarJWT(userFound) });
            }
        }

        [HttpGet]
        [Route("validarToken")] // Ruta: api/acceso/login
        public IActionResult ValidarToken([FromQuery]string token)
        {
            bool respuesta = _jwtUtil.validarToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta }); // Usuario no encontrado
        }

    }
}
