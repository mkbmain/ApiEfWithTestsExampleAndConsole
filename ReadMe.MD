
# Simple Ef code first api 
 using Sql (msSQL) with full di and test example

## generating the db
This repo uses code first to generate the db please navigate to datalayer folder and run command

*obviously change the data source userid,password,catalog etc to be what you wan them to be
```
dotnet-ef database update --connection "Data Source=localhost;Initial Catalog=ExampleDb;user id=sa;password=12345678"
```
or depending on version and how ef is configured
```
dotnet ef database update --connection "Data Source=localhost;Initial Catalog=ExampleDb;user id=sa;password=12345678"
```

## If EF is not setup on your machine
if ef has not been setup globally you can do so by running
```
dotnet tool install --global dotnet-ef
```

more details here https://docs.microsoft.com/en-us/ef/core/cli/dotnet



## Update the appsettings.json with url of your choice
