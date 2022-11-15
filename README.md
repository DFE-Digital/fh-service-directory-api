# fh-service-directory-api

## Requirements

* DotNet Core 6.0 and any supported IDE for DEV running.

## About

The Family Hubs service directory enables local authorities, voluntary, charitably, and faith organisations to list and manage their services available for vulnerable children and families.

This repos has been built using the "Clean Architecture Design" taken from Steve Smith (ardalis)

## Local running

In the appsetting.json file set the UseDbType with one of the following options:

* "UseInMemoryDatabase"
* "SqlServerDatabase"
* "Postgres"

The startup project is: FamilyHubs.ServiceDirectoryApi.Api
Starting the API will then show the swagger definition with the available operations.

## Migrations

To Add Migration
<br />
dotnet ef migrations add CreateIntialSchema -c ApplicationDbContext --output-dir ../FamilyHubs.ServiceDirectoryApi.Infrastructure/Persistence/Data
<br />
dotnet ef database update -c ApplicationDbContext
