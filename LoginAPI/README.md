# Web API con Autenticación JWT

### Crea los modelos de la base de dato
```bash
Scaffold-DbContext "Data Source=PC-MSI\\SQLEXPRESS;Initial Catalog=loginDoNet;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models
```

## Autenticación JWT en .NET

#### **1. Introducción a JWT (JSON Web Tokens)**
JWT (JSON Web Tokens) es un estándar abierto (RFC 7519) que define una forma compacta y autocontenida de transmitir información entre partes como un objeto JSON. Los tokens JWT se utilizan comúnmente para la autenticación y autorización en aplicaciones web modernas.

En una aplicación típica de autenticación con JWT, el servidor genera un token tras validar las credenciales del usuario, y este token se envía al cliente. Posteriormente, el cliente lo incluye en cada solicitud como prueba de autenticación. El servidor valida el token en cada solicitud para verificar la identidad del usuario.

#### **2. Componentes de un JWT**
Un JWT está compuesto por tres partes:
1. **Header**: Indica el tipo de token y el algoritmo de firma (generalmente HMAC o RSA).
2. **Payload**: Contiene los "claims" (afirmaciones), es decir, los datos del usuario y metadatos sobre la autenticación.
3. **Signature**: Verifica que el token no haya sido modificado.

Formato del JWT:
```
header.payload.signature
```

#### **3. Uso de JWT en .NET**
Para implementar JWT en una aplicación ASP.NET Core, es común seguir los siguientes pasos:

##### **Paso 1: Instalar el paquete NuGet**
Agrega el paquete `Microsoft.AspNetCore.Authentication.JwtBearer`:
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

##### **Paso 2: Configurar la autenticación JWT**
En el archivo `Startup.cs` o en el programa principal (`Program.cs` en versiones más recientes):

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

    services.AddAuthorization();
}
```

##### **Paso 3: Generar el token JWT**
Generalmente, cuando un usuario se autentica, el servidor genera un JWT con las credenciales del usuario:

```csharp
public string GenerateToken(User user)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

##### **Paso 4: Uso del token**
Una vez generado, el token se envía al cliente, y este debe incluirlo en el encabezado de las solicitudes HTTP en futuras interacciones:

```
Authorization: Bearer <token>
```

##### **Paso 5: Protección de endpoints**
Puedes proteger los endpoints del controlador en ASP.NET Core usando el atributo `[Authorize]`:

```csharp
[Authorize]
[HttpGet]
public IActionResult GetSecureData()
{
    return Ok("Este es un endpoint protegido.");
}
```

#### **4. Cuándo usar JWT**
El uso de JWT es recomendable en los siguientes escenarios:
- **Aplicaciones con autenticación sin estado (Stateless)**: JWT es ideal para sistemas sin estado, como APIs RESTful, donde la autenticación no depende de una sesión en el servidor.
- **Microservicios**: JWT es útil cuando necesitas que múltiples servicios o aplicaciones verifiquen la identidad de un usuario.
- **Autenticación distribuida**: Cuando diferentes servicios en diferentes plataformas necesitan autenticar usuarios de manera consistente.
- **Aplicaciones SPA (Single Page Applications)**: Para aplicaciones modernas que ejecutan gran parte de la lógica del cliente en el navegador y requieren autenticación.

#### **5. Ventajas y desventajas de JWT**
##### **Ventajas**:
1. **Autocontenido**: Toda la información necesaria para autenticar a un usuario está dentro del token. No es necesario mantener sesiones en el servidor.
2. **Sin estado**: No requiere almacenamiento en el servidor para mantener el estado de autenticación, lo que reduce la carga del servidor y facilita el escalado.
3. **Cross-platform**: Los tokens pueden ser generados y verificados por cualquier tecnología que soporte JWT.
4. **Control de caducidad**: Los tokens tienen un tiempo de vida limitado, lo que mejora la seguridad.
  
##### **Desventajas**:
1. **No revocación**: Una vez que un token se emite, no se puede revocar a menos que se implementen mecanismos adicionales como listas negras.
2. **Tamaño**: Los JWT pueden ser más grandes que otros métodos de autenticación porque incluyen todas las afirmaciones.
3. **Seguridad**: Si un token JWT se compromete, el atacante puede acceder a los recursos protegidos hasta que el token expire.

#### **6. Comparación con otros métodos de autenticación en .NET**
##### **Cookies de autenticación**
- **Cómo funciona**: El servidor genera una cookie en la autenticación y la envía al cliente. En cada solicitud, la cookie se envía de vuelta al servidor para verificar la identidad.
- **Usos**: Comúnmente en aplicaciones web tradicionales (por ejemplo, ASP.NET MVC) donde se mantiene el estado del usuario en el servidor.
- **Ventajas**: Soporte directo del navegador, revocación de sesión al cerrar sesión, capacidad de almacenamiento de datos más ligeros.
- **Desventajas**: Requiere estado en el servidor (sesiones), difícil de usar en APIs sin estado.

##### **OAuth 2.0 con OpenID Connect**
- **Cómo funciona**: OAuth 2.0 es un protocolo de autorización, mientras que OpenID Connect agrega una capa de autenticación sobre OAuth. Utiliza tokens para acceso y autenticación.
- **Usos**: Ideal para aplicaciones que requieren autorización delegada o autenticación a través de terceros (por ejemplo, Google, Facebook).
- **Ventajas**: Estándar bien aceptado, proporciona autenticación y autorización en un flujo.
- **Desventajas**: Puede ser más complejo de implementar que JWT o autenticación por cookies.

##### **Autenticación basada en Token (Sin JWT)**
- **Cómo funciona**: Similar a JWT, pero los tokens suelen ser almacenados y gestionados en el servidor, lo que implica mantener el estado.
- **Usos**: En aplicaciones que requieren mayor control sobre los tokens, como revocación.
- **Ventajas**: Mayor control sobre la validez y revocación de los tokens.
- **Desventajas**: No es sin estado, lo que implica mayor carga en el servidor.

#### **7. Conclusión**
La autenticación JWT en .NET es una opción poderosa para aplicaciones modernas sin estado, especialmente en el desarrollo de APIs y microservicios. Su naturaleza autocontenida y basada en tokens facilita la escalabilidad y la interoperabilidad entre diferentes plataformas. Sin embargo, en casos donde se requiere la revocación de tokens o el manejo de sesiones, otros enfoques como la autenticación por cookies o OAuth 2.0 pueden ser más adecuados.

El uso de JWT debe evaluarse según los requisitos del proyecto para aprovechar sus ventajas y mitigar sus limitaciones.

## Instalación de dependencias:
### Restaurar dependencias
```bash
dotnet restore
```
###  Verificar dependencias instaladas
```bash
dotnet list package
```