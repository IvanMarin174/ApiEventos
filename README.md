# Api Evnetos con .Net
### Instalación 🔧

_Ya que se clona, ejecutar_

```
dotnet restore
```

_Modificar appsettings.json en la Conexion_

```
"ConnectionStrings": {
  "Conexion": "server=;port=;uid=;pwd=;database=bd_eventos"
},
```

_Ejecutar las migraciones_
```
Update-Database
```
_Compilar proyecto_
```
dotnet build
```
_Compilar proyecto_
```
dotnet run
```
## Pruebas ⚙️

_Al ejecuatar las migracions se creo un usuario de pureba:_
```
ivan@correo.com
12345
```
