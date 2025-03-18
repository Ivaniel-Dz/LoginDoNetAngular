using Microsoft.AspNetCore.Authentication.JwtBearer; // Para especificar el esquema de autenticación JWT
using Microsoft.AspNetCore.Authorization; // Para manejar la autorización basada en autenticación
using Microsoft.AspNetCore.Mvc; // Para construir controladores de API y acciones
using Microsoft.EntityFrameworkCore; // Para trabajar con Entity Framework y realizar operaciones de base de datos
using LoginAPI.Data; // Referencia a los modelos de la aplicación, en este caso la entidad Producto

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")] // Ruta para las peticiones del controlador (e.g. api/producto)
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Requiere autenticación JWT para acceder
    [ApiController] // Indica que es un controlador de API que gestiona respuestas JSON automáticamente
    public class ProductoController : ControllerBase
    {
        private readonly AppDBContext _appDBContext; // Inyección del contexto de la base de datos
        public ProductoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext; // Asignación del contexto inyectado
        }

        [HttpGet]
        [Route("Lista")] // Ruta de la acción: api/producto/lista
        public async Task<IActionResult> lista()
        {
            // Obtener la lista de productos de la base de datos de forma asíncrona
            var lista = await _appDBContext.Productos.ToListAsync();
            // Retornar una respuesta HTTP 200 OK con la lista de productos
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }
    }
}
