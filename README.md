# Tfemshoes Api

API for the tfem shoes frontend

## Getting started

Uses .NET 6 and WebAPI to host a RESTful service for the shoes website.

Visual Studio 2022 in recommended with the .NET 6 SDK to build and run the project locally. A dev site will be hosted soon as well.

### Secrets
This project uses Azure Key Vault to store secrets for the deployed environments. This setting is controlled by the `KeyVaultName` property in `appsettings.json`.

For local development, the project uses .NET Core's SecretsManager to locally store individual secrets. However, for the project to run, you will need to provide the following:
* DatabaseServerName
* DatabaseName
* DatabaseUserId
* DatabasePassword

Either create your own local database (SQL Server flavor) or ask the project team to grant access to the dev database in Azure.
Secrets can be created using the the [SecretsManager CLI](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#set-a-secret) commands.
```
dotnet user-secrets set "<SecretKey>" "<SecretValue>"
```
