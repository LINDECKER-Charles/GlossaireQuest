using TechQuiz.Api.Data;
using TechQuiz.Api.Services; // ✅ Ajoute ceci pour accéder à JwtService
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Ajoute le support des contrôleurs
builder.Services.AddControllers();

// Ajoute Entity Framework + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Clé secrète utilisée pour signer le token
var secretKey = "71d6f722f2725e62ce44ec1caec3701245e2b0fbc4e506b307bff50d05f69f9bc752b0dd";
var key = Encoding.ASCII.GetBytes(secretKey);

// ✅ Enregistre ton service JWT dans le conteneur DI
builder.Services.AddSingleton(new JwtService(secretKey));

// Configure l’authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
            .AllowCredentials();
    });
});



// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AccesService>();
builder.Services.AddScoped<QuizzService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);



var app = builder.Build();

// Active Swagger uniquement en environnement dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowFrontend");

/* app.UseHttpsRedirection(); */

// ✅ Ces deux middlewares doivent venir avant MapControllers()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
