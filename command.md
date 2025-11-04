dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet run
dotnet restore;dotnet build; dotnet run;
dotnet watch run