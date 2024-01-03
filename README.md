# Lector DBF a SQLite

Este proyecto lee archivos DBF y los importa a una base de datos SQLite.

## Descripci�n 

El c�digo realiza las siguientes acciones:

- Lee el archivo DBF especificado usando la librer�a DbfDataReader
- Crea una nueva base de datos SQLite si no existe
- Crea una tabla en SQLite con el nombre del archivo DBF 
- Lee cada registro del archivo DBF
  - Omite registros eliminados si se indica skipDeleted
  - Inserta cada registro como una fila en la tabla SQLite
- Envuelve las operaciones SQLite en una transacci�n para mejor performance

## Uso

Para ejecutar el programa:

```
dotnet run
```

Se debe especificar la ruta al archivo DBF en la variable dbfPath.

Por defecto crear� la base de datos SQLite en la misma carpeta.

## Mejoras Pendientes

Algunas mejoras que se podr�an realizar:

- Parametrizar la query INSERT para evitar inyecci�n SQL
- Manejo de errores y excepciones
- Detectar tipos de datos en DBF y mapear a tipos SQLite
- Tests unitarios
- Opciones de l�nea de comando para archivo DBF, base de datos SQLite, etc

## Licencia

Este c�digo se provee como ejemplo sin ninguna garant�a bajo la licencia [MIT](https://choosealicense.com/licenses/mit/).