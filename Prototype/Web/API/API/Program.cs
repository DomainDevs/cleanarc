using API.Configurations;
using Application;
using Infrastructure;
using Persistence;
using Serilog;
using System.Linq.Expressions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations(); //Cargar archivos de configuración json
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration); //Inyectar capa infraestructura
    builder.Services.AddPersistence(builder.Configuration); //Inyectar capa persistencia
    builder.Services.AddApplication();


    builder.Services.AddOpenApi();

    var app = builder.Build();
    //if (app.Environment.IsDevelopment()) { app.MapOpenApi(); } // Configure the HTTP request pipeline.
    app.UseInfrastructure(builder.Configuration);
    app.MapControllers();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Console.WriteLine("Error iniciando el servicio", ex.Message);
}
finally
{ 
}


