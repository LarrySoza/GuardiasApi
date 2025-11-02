using App.WebApi.Entities;
using App.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.SignalR;
using NLog;
using NLog.Web;
namespace App.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.ConfigureCors();
                builder.Services.AddInfrastructure();
                builder.Services.ConfigureAuthJwt(builder.Configuration);
                builder.Services.AddControllers(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseUrls()));
                });

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                builder.Services.ConfigureSwagger();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                });

                //Registrar el servicio singleton para rastrear usuarios conectados
                builder.Services.AddSingleton<ConnectedUsersTracker>();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                //if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guardias.WebApi v1"));
                }

                //Usar el manejador de error
                app.ConfigureExceptionHandler();

                app.UseRouting();

                app.UseCors("CorsPolicy");

                app.UseAuthentication();

                app.UseAuthorization();

                app.MapControllers();

                app.ConfigureHubs();

                app.Run();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                logger.Error(ex, "Programa detenido debido a excepción");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
