
## Installation

Install Nuget Package (WebAPI)

```bash
  1 dotnet add package Dapper
  2 dotnet add package Microsoft.Data.SqlClient 
```
    
## Roadmap

- create ClassLibrary
- create ConsoleApp
- create WebAPI

## Solution 

- dotnet new sln -n MiniPOS_DapperAPI
- dotnet sln add MiniPOS_WebAPI/MiniPOS_WebAPI.csproj
- dotnet sln add MiniPOS_ClassLib/MiniPOS_ClassLib.csproj
- dotnet sln add MiniPOS_ConsoleApp/MiniPOS_ConsoleApp.csproj

## Dependency Injection (DI)

- cd ../MiniPOS_WebAPI
  dotnet add reference ../MiniPOS_ClassLib/MiniPOS_ClassLib.csproj

## Create Database

- with Dapper 





