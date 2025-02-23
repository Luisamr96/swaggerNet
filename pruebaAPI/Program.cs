using Dapper.FluentMap;
using pruebaAPI.DTO;

var builder = WebApplication.CreateBuilder(args);

// Registra el mapeo de Dapper
FluentMapper.Initialize(config =>
{
    config.AddMap(new LogDtoMapp());
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Añadir Swagger

var app = builder.Build();
// Configurar la canalización HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Usar Swagger
    app.UseSwaggerUI(); // Interfaz de usuario de Swagger
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
