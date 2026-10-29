using Microsoft.Extensions.Options;
using DSW_3P_PR01.Repositories;
using DSW_3P_PR01.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar OpenAPI.
builder.Services.AddOpenApi();

// Habilitar los controladores.
builder.Services.AddControllers();

// Configurar la inyección de dependencias.
builder.Services.AddScoped<IMisionRepository, MisionJsonRepository>();
builder.Services.AddScoped<IMisionService, MisionService>();

var app = builder.Build();

// Configurar el entorno de desarrollo.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "QuestLog API v1"
        );
    });
}

// Configurar las rutas de la aplicación.
app.UseHttpsRedirection();

app.MapControllers();

app.Run();