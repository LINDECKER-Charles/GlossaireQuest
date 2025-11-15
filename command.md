dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet run
dotnet restore;dotnet build; dotnet run;
dotnet watch run

ng g c dossier/nom 
ng g s <nom>
ng g i models/<nom> --type=model
ng g class <nom>
ng g enum <nom>
ng g m <nom>
ng g guard <nom>
ng g guard <nom>
ng g interceptor <nom>
