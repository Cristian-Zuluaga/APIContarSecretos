# APIContarSecretos
Proyecto desarrollado para gestion de biblioteca virtual

# Arquitectura
Proyecto desarrollado bajo arquitectura NCapas, la cual se desarrolla en tres capas.
* Capa API: Exposicion de servicios web (Api Rest)
* Capa Business: Capa de negocio, se encuntran servicios y logica de negocio.
* Capa Data: Capa de manejo de datos y manipulacion con EF Core

# Tecnologias
* .NET Core 9
* Entity Framework Core 9
* Postgree SQL
* JWT


# Creacion Usuario admin
Se encuentra en proyecto API en archivo program.cs, se invoca metodo PopulateDB, esto solo se usa para creacion de usuario admin principal.