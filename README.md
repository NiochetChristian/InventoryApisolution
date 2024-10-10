## SOUCION QUE CONTIENE 2 PROYECTOS BASADOS EN .NET 8.0 ##
* Primeros pasos
  - Cada proyecto tiene un appSettings, la cual tanto uno como el otro tiene que tener la misma informacion (para mantener el orden en debug como en release)
  - El proyecto "InventoryClientAPI" contiene tanto el connectionString como una propiedad llamada "Token"
     + connecctionString: tiene que ser una cadena conformada
        1 por ip de servidor
        2 base de datos
        3 usuario (preferible root)
        4 contraseña (en blanco si no tiene contraseña el usuario)
      quedando en una cadena asi: Server=localhost;Database=nombreTabla;User Id=usuario;Password=contraseña;Charset=utf8mb4;

  - El proyecto "ClientApp" contiene tanto el token como el apiUrl (que esta por defecto)
     + El token se extrae de la llamada (no requiere autorizacion) "api/Token" que genera un token de alto limite de expiracion. Este endpoint se genera al iniciar el proyecto InventoryClientAPI y puede tanto llamarse por url como en swagger
     + El ApiUrl contiene la direccion de la url base de la api, tener en cuenta que la direccion tiene que terminar con un '/', por ejemplo: localhost:1000/api/Products/.

  - Al abrir la solucion se requiere obligatoriamente configurar la base de datos la cual se hara la migracion/actualizacion. Con estos comandos (y el connectionString bien construido) deberia de funcionar bien acorde al proyecto
     * Add-Migration InitialCreate
     * Update-Database


* Cosas a acotar:
  - El proyecto no tiene sesiones, por eso implemente el JWT como unico valor la cual requiere las api y siendo este 'libre' para realizar la prueba tecnica
  - El tipo de autoenticacion del JWT es "Bearer"
  - Todos los endpoint requieren del token a exceptuar el de 'api/token'
  - Por defecto, la conexion esta como: "Server=;Database=InventoryDB;User Id=root;Password=;Charset=utf8mb4;" solo para agregar el servidor, el usuario y la contraseña
