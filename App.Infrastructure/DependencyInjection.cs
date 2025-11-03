using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.Infrastructure.Database;
using App.Infrastructure.Repository;
using App.Infrastructure.Repository.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar fábrica de conexiones (una instancia lee la cadena y la guarda)
            services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();

            services.AddTransient<IAliveCheckEstadoRepository, AliveCheckEstadoRepository>();
            services.AddTransient<IAliveCheckRepository, AliveCheckRepository>();
            services.AddTransient<IAliveCheckRespuestaRepository, AliveCheckRespuestaRepository>();

            services.AddTransient<IAsignacionEventoRepository, AsignacionEventoRepository>();
            services.AddTransient<IAsignacionEventoTipoRepository, AsignacionEventoTipoRepository>();
            services.AddTransient<IAsignacionRepository, AsignacionRepository>();

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteUsuarioRepository, ClienteUsuarioRepository>();

            services.AddTransient<IConfiguracionRepository, ConfiguracionRepository>();

            services.AddTransient<IControlPointRepository, ControlPointRepository>();

            services.AddTransient<IIncidenciaAdjuntoRepository, IncidenciaAdjuntoRepository>();
            services.AddTransient<IIncidenciaRepository, IncidenciaRepository>();
            services.AddTransient<IIncidenteTipoRepository, IncidenteTipoRepository>();

            services.AddTransient<IOcurrenciaAdjuntoRepository, OcurrenciaAdjuntoRepository>();
            services.AddTransient<IOcurrenciaRepository, OcurrenciaRepository>();
            services.AddTransient<IOcurrenciaTipoRepository, OcurrenciaTipoRepository>();

            services.AddTransient<IPanicAlertAdjuntoRepository, PanicAlertAdjuntoRepository>();
            services.AddTransient<IPanicAlertAdjuntoTipoRepository, PanicAlertAdjuntoTipoRepository>();
            services.AddTransient<IPanicAlertEstadoRepository, PanicAlertEstadoRepository>();
            services.AddTransient<IPanicAlertRecepcionRepository, PanicAlertRecepcionRepository>();
            services.AddTransient<IPanicAlertRepository, PanicAlertRepository>();

            services.AddTransient<IPuestoRepository, PuestoRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
            services.AddTransient<IRondaAdjuntoRepository, RondaAdjuntoRepository>();
            services.AddTransient<IRondaRepository, RondaRepository>();

            services.AddTransient<ISesionUsuarioEvidenciaRepository, SesionUsuarioEvidenciaRepository>();
            services.AddTransient<ISesionUsuarioRepository, SesionUsuarioRepository>();

            services.AddTransient<ITipoConfiguracionRepository, TipoConfiguracionRepository>();
            services.AddTransient<IUnidadRepository, UnidadRepository>();
            services.AddTransient<IUsuarioEstadoRepository, UsuarioEstadoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioRolRepository, UsuarioRolRepository>();
            services.AddTransient<IUsuarioUnidadRepository, UsuarioUnidadRepository>();

            // Registrar UnitOfWork como Scoped
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
