# Lector DBF a SQLite

Este proyecto lee archivos DBF y los importa a una base de datos SQLite.

## Descripción 

El código realiza las siguientes acciones:

- Lee el archivo DBF especificado usando la librería DbfDataReader
- Crea una nueva base de datos SQLite si no existe
- Crea una tabla en SQLite con el nombre del archivo DBF 
- Lee cada registro del archivo DBF
  - Omite registros eliminados si se indica skipDeleted
  - Inserta cada registro como una fila en la tabla SQLite
- Envuelve las operaciones SQLite en una transacción para mejor performance

## Uso

Para ejecutar el programa:

```
dotnet run
```

Se debe especificar la ruta al archivo DBF en la variable dbfPath.

Por defecto creará la base de datos SQLite en la misma carpeta.

## Mejoras Pendientes

Algunas mejoras que se podrían realizar:

- Parametrizar la query INSERT para evitar inyección SQL
- Manejo de errores y excepciones
- Detectar tipos de datos en DBF y mapear a tipos SQLite
- Tests unitarios
- Opciones de línea de comando para archivo DBF, base de datos SQLite, etc

## Licencia

Este código se provee como ejemplo sin ninguna garantía bajo la licencia [MIT](https://choosealicense.com/licenses/mit/).