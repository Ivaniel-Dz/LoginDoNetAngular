# Login Client (Frontend)

## Conceptos Principales de un proyecto de Angular

### 1. **Guards** 🛑  
   - **Función:** Controlan el acceso a rutas.  
   - **Cuándo usarlos:** Para restringir acceso a usuarios no autenticados o sin permisos.  
   - **Contenido:** Implementan `CanActivate`, `CanDeactivate`, `CanLoad`, etc.

### 2. **Interceptor** 🛠️  
   - **Función:** Modifican o interceptan solicitudes HTTP.  
   - **Cuándo usarlos:** Para añadir tokens de autenticación, logs o manejar errores globales.  
   - **Contenido:** Implementa `HttpInterceptor` y se usa con `HTTP_INTERCEPTORS`.

### 3. **Interfaces** 📄  
   - **Función:** Definen estructuras de datos.  
   - **Cuándo usarlas:** Para tipar datos y mejorar el código.  
   - **Contenido:** Definen propiedades (`id: number`, `name: string`, etc.).

### 4. **Services** 🔄  
   - **Función:** Manejan lógica de negocio y llamadas a APIs.  
   - **Cuándo usarlos:** Para compartir datos y lógica en toda la app.  
   - **Contenido:** Métodos como `getUsers()`, `postUser(data)`, etc.

### 5. **Repositories** 🗄️  
   - **Función:** Abstracción para interactuar con APIs o bases de datos.  
   - **Cuándo usarlos:** Para centralizar llamadas HTTP en un solo lugar.  
   - **Contenido:** Métodos como `fetchUsers()`, `createUser(user)`. 

### 6. **Settings** ⚙️  
   - **Función:** Almacena configuraciones globales.  
   - **Cuándo usarlos:** Para constantes como URLs de API, claves de configuración.  
   - **Contenido:** Variables como `API_URL`, `APP_VERSION`.

### 7. **Custom** 🎨  
   - **Función:** Contiene código personalizado como pipes, directivas o utilidades.  
   - **Cuándo usarlos:** Para funcionalidades específicas reutilizables.  
   - **Contenido:** Pipes (`CustomDatePipe`), directivas (`HighlightDirective`).

### 8. **app.router** 🚏  
   - **Función:** Define las rutas de la aplicación.  
   - **Cuándo usarlo:** Para configurar navegación y lazy loading.  
   - **Contenido:** Array de rutas con `{ path: '', component: HomeComponent }`.

### 9. **Environments** 🌍  
   - **Función:** Gestiona configuraciones para distintos entornos (dev, prod).  
   - **Cuándo usarlo:** Para cambiar URLs o flags según el ambiente.  
   - **Contenido:** Archivos `environment.ts` con `production: false`. 

--------------------------------------------------------
## Configuración del Proyecto en Local
- Clona el proyecto
- Instalar Angular CLI (si no lo tienes)

### Instalación de Dependencias del Proyecto
```bash
npm install
```

###  Verifica Angular Material
- Si el proyecto usa Angular Material, asegúrate de que esté correctamente configurado.
- Si necesitas agregar Angular Material a un proyecto existente, puedes hacerlo con:
```bash
ng add @angular/material
```
>Esto te guiará a través de la configuración de Angular Material, incluyendo la selección de un tema y la configuración de animaciones.

### Ejecuta el proyecto
```bash
ng serve
```

> Esto levantará un servidor de desarrollo y podrás acceder a la aplicación en ``http://localhost:4200``.