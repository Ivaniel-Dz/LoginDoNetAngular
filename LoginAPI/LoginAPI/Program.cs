using Microsoft.AspNetCore.Authentication.JwtBearer; // Para manejar autenticaci�n con JWT (JSON Web Tokens)
using Microsoft.EntityFrameworkCore; // Para manejar la base de datos utilizando Entity Framework Core
using Microsoft.IdentityModel.Tokens; // Para configurar la validaci�n de los tokens JWT
using System.Text; // Para manejar codificaci�n de cadenas en bytes
using LoginAPI.Data; // Referencia a utilidades personalizadas
using LoginAPI.Jwt; // Referencia a los modelos de la aplicaci�n

var builder = WebApplication.CreateBuilder(args); // Crear la aplicaci�n con la configuraci�n inicial

// Agregar controladores al servicio
builder.Services.AddControllers();

// Swagger: Herramienta para generar documentaci�n autom�tica de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de la base de datos (SQL Server)
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")); // Obtener cadena de conexi�n de configuraci�n
});

// Registrar servicios personalizados
builder.Services.AddSingleton<JwtUtil>(); // Servicio singleton para utilidades personalizadas

// Configuraci�n de la autenticaci�n con JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema predeterminado de autenticaci�n
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Esquema predeterminado para desaf�os de autenticaci�n
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false; // No requiere HTTPS (�til en desarrollo)
    config.SaveToken = true; // Permitir que el servidor guarde el token
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validar la clave de firma del emisor
        ValidateIssuer = false, // No validar el emisor (puedes cambiarlo seg�n sea necesario)
        ValidateAudience = false, // No validar la audiencia (puedes cambiarlo seg�n sea necesario)
        ValidateLifetime = true, // Validar que el token no est� expirado
        ClockSkew = TimeSpan.Zero, // No permitir margen de tiempo adicional para expiraci�n
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!)) // Clave de firma del token
    };
});

// Habilitar CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options => {
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // Permitir solicitudes de cualquier origen, encabezado o m�todo
    });
});

var app = builder.Build(); // Construir la aplicaci�n

// Configuraci�n del pipeline HTTP en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Usar Swagger para documentaci�n
    app.UseSwaggerUI(); // Interfaz de usuario de Swagger
}

// Habilitar CORS con la pol�tica definida
app.UseCors("NewPolicy");

// Configurar autenticaci�n y autorizaci�n
app.UseAuthentication(); // Middleware de autenticaci�n
app.UseAuthorization(); // Middleware de autorizaci�n

// Mapear las rutas de los controladores
app.MapControllers();

app.Run(); // Ejecutar la aplicaci�n

