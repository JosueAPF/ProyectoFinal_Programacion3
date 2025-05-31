using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Proyecto_Final.Servicio;
using Proyecto_Final.Utils;
using Proyecto_Final.ArmadoEstructuras;

//using web_Api_persona.Servicios; //importa los servicios

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


// ⬇️ Instancias unicas de mis servicios y clases Especiales

//Utiles:  Ruta-Deserealizador- primera prueba de servicio
builder.Services.AddSingleton<Rutas>();
builder.Services.AddTransient(typeof(DeserealizadorGenerico<>));
//builder.Services.AddSingleton<PruebaServicio>();

/*A prueba servicios */
builder.Services.AddSingleton<ContextDatos>();
builder.Services.AddSingleton<ClientesServicio>();
builder.Services.AddSingleton<ServicioUsuariosActivos>();
builder.Services.AddSingleton<TarjetaServicio>();
builder.Services.AddSingleton<TransaccionServicio>();
builder.Services.AddSingleton<ResumenServicio>();


// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Proyecto Final",
        Version = "v1",
        Description = @"
API REST para gestión de Tarjetas de Crédito

**Funcionalidades principales:**

1. Gestión de Clientes      
    - ABB CRUD
    - AVL Login balanceado
    - Tabla Hash Resumen
 

2. Gestión de Tarjetas  y consultas
    - Cola CRUD
    - AVL listado de Eliminados
    - Lista Enlazada
        - listado Pagos
        - listado Compras

3. Gestion de Transacciones  
    - Cola Pendientes
    - Pila Recientes
    - ABB ver Todo y Busqueda
 
"
    });
});
var app = builder.Build();


// 1) Habilita servir archivos estáticos en wwwroot/
app.UseStaticFiles();

//var pathOfSwaggerUi = "/swagger-ui";
// Redirigir la raíz "/" hacia "/swagger"

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cojunto de Apis Clientes-Tarjetas-Transacciones");

        c.RoutePrefix = string.Empty;





        c.InjectStylesheet("/swagger-ui/custom.css");



    });
}

/*

if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger
    app.UseSwagger();

    // Habilitar Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
        c.RoutePrefix = "swagger";
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


/*
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});
*/


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();