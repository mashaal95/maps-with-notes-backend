# Project Setup

## SQL Database Initialization

### 1. Pull the SQL Server Docker image:
```powershell
docker pull mcr.microsoft.com/mssql/server:2022-latest
```
### 2. Initialize the SQL Server container:
```powershell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=7Times=10!" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Access the container's shell:
```powershell
docker exec -it sql1 "bash"
```
### 4. Connect to SQL Server:
```powershell
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "7Times=10!"
```
## Data Import

### 1. Initialize the database:
```sql
create database TestDb
```

### 2. Create tables for Map Data:
```sql
CREATE TABLE [dbo].[UserTable] (
    [user_id]  UNIQUEIDENTIFIER NOT NULL,
    [username] NVARCHAR (50)    NOT NULL,
    [password] VARCHAR (MAX)    NOT NULL,
    CONSTRAINT [PK_UserTable] PRIMARY KEY CLUSTERED ([user_id] ASC)
);
CREATE TABLE [dbo].[NotesTable] (
    [message_id]    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [user_id]       UNIQUEIDENTIFIER NOT NULL,
    [location_name] NVARCHAR (MAX)   NOT NULL,
    [notes_text]    NVARCHAR (MAX)   NOT NULL,
);

ALTER TABLE [dbo].[NotesTable] ADD FOREIGN KEY (user_id) REFERENCES [dbo].[UserTable] (user_id)

```
### Insert some sample users:
``` sql
INSERT into UserTable VALUES ('3fa85f64-5717-4562-b3fc-2c963f66afa6','mashaal95','hello')
INSERT into UserTable VALUES ('13bfe2d7-73d4-4971-8839-5f7f298774d6','dennis1','hello')
INSERT into UserTable VALUES ('def99cfb-a38c-4c6d-ba40-827c3fc530aa','zar93','hello')

```

## Web API Setup

### Create a new .NET Core 7 Web API project:
```powershell
dotnet new webapi -o SeifaAPI
```

### Install the required NuGet packages:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.Design  
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```
### Scaffold the models based on the existing database tables:
```powershell
dotnet ef dbcontext scaffold "Name=ConnectionStrings:TestDb" Microsoft.EntityFrameworkCore.SqlServer
```

### Scaffold the sample controllers:
``` powershell
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 7.0.0
    dotnet add package Microsoft.EntityFrameworkCore.Design -v 7.0.0
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 7.0.0
    dotnet tool uninstall -g dotnet-aspnet-codegenerator
    dotnet tool install -g dotnet-aspnet-codegenerator
    dotnet-aspnet-codegenerator controller -name NotesController -async -api -m NotesTable -dc TestDbContext -outDir Controllers
    dotnet-aspnet-codegenerator controller -name UserController -async -api -m UserTable -dc TestDbContext -outDir Controllers
```

### Additional Notes

Ensure that the Docker based SQL Server container is running and accessible before running the application.
Adjust the file paths in the docker cp commands based on your local file locations.
Modify the connection string in the dotnet ef dbcontext scaffold command to match your SQL Server credentials.
