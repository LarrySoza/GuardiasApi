using App.Application.Interfaces.Core;

namespace App.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAdjuntoTipoRepository AdjuntoTipos { get; }
        IAliveCheckEstadoRepository AliveCheckEstados { get; }
        IAliveCheckRepository AliveChecks { get; }
        IAliveCheckRespuestaRepository AliveCheckRespuestas { get; }
        IAsignacionEventoRepository AsignacionEventos { get; }
        IAsignacionEventoTipoRepository AsignacionEventoTipos { get; }
        IAsignacionRepository Asignaciones { get; }
        IClienteRepository Clientes { get; }
        IClienteUsuarioRepository ClienteUsuarios { get; }
        IConfiguracionRepository Configuraciones { get; }
        IControlPointRepository ControlPoints { get; }
        IEventoAdjuntoRepository EventoAdjuntos { get; }
        IEventoRepository Eventos { get; }
        IEventoTipoRepository EventoTipos { get; }
        IPanicAlertAdjuntoRepository PanicAlertAdjuntos { get; }
        IPanicAlertEstadoRepository PanicAlertEstados { get; }
        IPanicAlertRecepcionRepository PanicAlertRecepciones { get; }
        IPanicAlertRepository PanicAlerts { get; }
        IPuestoRepository Puestos { get; }
        IRolRepository Roles { get; }
        IRondaAdjuntoRepository RondaAdjuntos { get; }
        IRondaRepository Rondas { get; }
        ISesionUsuarioEvidenciaRepository SesionUsuarioEvidencias { get; }
        ISesionUsuarioRepository SesionUsuarios { get; }
        ITipoConfiguracionRepository TipoConfiguraciones { get; }
        ITipoDocumentoRepository TipoDocumento { get; }
        ITurnoRepository Turnos { get; }
        IUnidadRepository Unidades { get; }
        IUsuarioEstadoRepository UsuarioEstados { get; }
        IUsuarioPuestoRepository UsuarioPuestos { get; }
        IUsuarioRepository Usuarios { get; }
        IUsuarioRolRepository UsuarioRoles { get; }
        IUsuarioUnidadRepository UsuarioUnidades { get; }
    }
}
