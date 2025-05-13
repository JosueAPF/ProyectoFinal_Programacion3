using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Proyecto_Final.Servicio;
using Proyecto_Final.Utils;
using Proyecto_Final.ArmadoEstructuras;

//using web_Api_persona.Servicios; //importa los servicios

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


// ⬇️  registras el servicio y Otras clases relevanes mientras en el contructor pidan datos
//builder.Services.AddSingleton<PilaServicio>();
builder.Services.AddSingleton<Rutas>();
builder.Services.AddTransient(typeof(DeserealizadorGenerico<>));
builder.Services.AddSingleton<PruebaServicio>();

/*A prueba*/
builder.Services.AddSingleton<ContextDatos>();
builder.Services.AddSingleton<ClientesServicio>();
builder.Services.AddSingleton<TarjetaServicio>();
builder.Services.AddSingleton<TransaccionServicio>();





// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Proyecto Final",
        Version = "v1",
        Description = "API REST para gestión de Tarjetas de Credito"
    });
});


var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger
    app.UseSwagger();

    // Habilitar Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
        c.RoutePrefix = "swagger"; // Acceder desde /swagger
    });
}

// Redirigir la raíz "/" hacia "/swagger"
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();