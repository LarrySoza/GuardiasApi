using App.Application.Interfaces.Core;

namespace App.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAliveCheckEstadoRepository AliveCheckEstados { get; }
        IAliveCheckRepository AliveChecks { get; }
        IAliveCheckRespuestaRepository AliveCheckRespuestas { get; }
        IAsignacionEventoRepository AsignacionEventos { get; }
        IAsignacionEventoTipoRepository AsignacionEventoTipos { get; }
        IAsignacionRepository Asignaciones { get; }
        IClienteUsuarioRepository ClienteUsuarios { get; }
        IClienteRepository Clientes { get; }
        IConfiguracionRepository Configuraciones { get; }
        IControlPointRepository ControlPoints { get; }
        IIncidenciaAdjuntoRepository IncidenciaAdjuntos { get; }
        IIncidenciaRepository Incidencias { get; }
        IIncidenteTipoRepository IncidenteTipos { get; }
        IOcurrenciaAdjuntoRepository OcurrenciaAdjuntos { get; }
        IOcurrenciaRepository Ocurrencias { get; }
        IOcurrenciaTipoRepository OcurrenciaTipos { get; }
        IPanicAlertAdjuntoTipoRepository PanicAlertAdjuntoTipos { get; }
        IPanicAlertAdjuntoRepository PanicAlertAdjuntos { get; }
        IPanicAlertEstadoRepository PanicAlertEstados { get; }
        IPanicAlertRepository PanicAlerts { get; }
        IPanicAlertRecepcionRepository PanicAlertRecepciones { get; }
        IPuestoRepository Puestos { get; }
        IRondaAdjuntoRepository RondaAdjuntos { get; }
        IRondaRepository Rondas { get; }
        IRolRepository Roles { get; }
        ISesionUsuarioEvidenciaRepository SesionUsuarioEvidencias { get; }
        ISesionUsuarioRepository SesionUsuarios { get; }
        ITipoConfiguracionRepository TipoConfiguraciones { get; }
        IUnidadRepository Unidades { get; }
        IUsuarioEstadoRepository UsuarioEstados { get; }
        IUsuarioRolRepository UsuarioRoles { get; }
        IUsuarioUnidadRepository UsuarioUnidades { get; }
        IUsuarioRepository Usuarios { get; }
    }
}
