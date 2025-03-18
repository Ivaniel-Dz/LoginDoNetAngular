using Microsoft.AspNetCore.Authentication.JwtBearer; // Para manejar autenticación con JWT (JSON Web Tokens)
using Microsoft.EntityFrameworkCore; // Para manejar la base de datos utilizando Entity Framework Core
using Microsoft.IdentityModel.Tokens; // Para configurar la validación de los tokens JWT
using System.Text; // Para manejar codificación de cadenas en bytes
using LoginAPI.Data; // Referencia a utilidades personalizadas
using LoginAPI.Jwt; // Referencia a los modelos de la aplicación

var builder = WebApplication.CreateBuilder(args); // Crear la aplicación con la configuración inicial

// Agregar controladores al servicio
builder.Services.AddControllers();

// Swagger: Herramienta para generar documentación automática de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la base de datos (SQL Server)
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")); // Obtener cadena de conexión de configuración
});

// Registrar servicios personalizados
builder.Services.AddSingleton<JwtUtil>(); // Servicio singleton para utilidades personalizadas

// Configuración de la autenticación con JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema predeterminado de autenticación
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema predeterminado para desafíos de autenticación
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false; // No requiere HTTPS (útil en desarrollo)
    config.SaveToken = true; // Permitir que el servidor guarde el token
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validar la clave de firma del emisor
        ValidateIssuer = false, // No validar el emisor (puedes cambiarlo según sea necesario)
        ValidateAudience = false, // No validar la audiencia (puedes cambiarlo según sea necesario)
        ValidateLifetime = true, // Validar que el token no esté expirado
        ClockSkew = TimeSpan.Zero, // No permitir margen de tiempo adicional para expiración
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!)) // Clave de firma del token
    };
});

// Habilitar CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options => {
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // Permitir solicitudes de cualquier origen, encabezado o método
    });
});

var app = builder.Build(); // Construir la aplicación

// Configuración del pipeline HTTP en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Usar Swagger para documentación
    app.UseSwaggerUI(); // Interfaz de usuario de Swagger
}

// Habilitar CORS con la política definida
app.UseCors("NewPolicy");

// Configurar autenticación y autorización
app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization(); // Middleware de autorización

// Mapear las rutas de los controladores
app.MapControllers();

app.Run(); // Ejecutar la aplicación

