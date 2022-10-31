# fh-service-directory-api

## Requirements

* DotNet Core 6.0 and any supported IDE for DEV running.

## About

The Family Hubs service directory enables local authorities, voluntary, charitably, and faith organisations to list and manage their services available for vulnerable children and families.

This repos has been built using the "Clean Architecture Design" taken from Steve Smith (ardalis)

## Local running

In the appsetting.json file set:

To use In Memory Database set the following:

* "RecreateDbOnStartup": true,
* "UseInMemoryDatabase": true,
* "UseSqlServerDatabase": false

To use a SQL Server database ensure the DefaultConnection is set then set:

* "RecreateDbOnStartup": false,
* "UseInMemoryDatabase": false,
* "UseSqlServerDatabase": true

To use Postgres database ensure the DefaultConnection is set then set:

* "RecreateDbOnStartup": false,
* "UseInMemoryDatabase": false,
* "UseSqlServerDatabase": false

The startup project is: FamilyHubs.ServiceDirectoryApi.Api
Starting the API will then show the swagger definition with the available operations.

