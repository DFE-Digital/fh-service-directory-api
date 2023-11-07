# fh-service-directory-api

## Requirements

* DotNet Core 7.0 and any supported IDE for DEV running.

## About

The Family Hubs service directory enables local authorities, voluntary, charitably, and faith organisations to list and manage their services available for vulnerable children and families.

This repos has been built using the "Clean Architecture Design" taken from Steve Smith (ardalis)

## Local running

In the appsetting.json file set the UseDbType with one of the following options:

* "SqlServerDatabase"

The startup project is: FamilyHubs.ServiceDirectory.Api
Starting the API will then show the swagger definition with the available operations.

## Migrations

To Add Migration

<br />
 dotnet ef migrations add CreateIntialSchema --project ..\FamilyHubs.ServiceDirectory.data
<br />

To Apply Latest Schema Manually

<br />
 dotnet ef database update --project ..\FamilyHubs.ServiceDirectory.data
<br />

## cypress tests
Run the API (debug or non debug both fine)
open powershell at ..\fh-service-directory-api\tests\cypress

<br />
 npx cypress open 
<br />

