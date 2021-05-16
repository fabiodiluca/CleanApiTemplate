#dotnet tool install -g FluentMigrator.DotNet.Cli
dotnet fm migrate -p SqlServer2016 -c "Server=localhost;Database=CleanTemplate;User Id=CleanTemplate;Password=CleanTemplate;" -a Migrations.dll