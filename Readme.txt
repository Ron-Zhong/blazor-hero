Install-Package Microsoft.EntityFrameworkCore.SqlServer
dotnet tool update --global dotnet-ef
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools

Scaffold-DbContext "Server=localhost;database=devmedudb;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities
