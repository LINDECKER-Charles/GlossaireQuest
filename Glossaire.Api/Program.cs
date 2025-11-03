var builder = WebApplication.CreateBuilder(args);

// Ajoute le support des contrôleurs (obligatoire si tu utilises app.MapControllers)
builder.Services.AddControllers();

// (Optionnel) Documentation OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Active Swagger uniquement en environnement dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Route les contrôleurs
app.MapControllers();

app.Run();
