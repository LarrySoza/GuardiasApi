using App.Application.Interfaces;
using App.Application.Interfaces.Core;

namespace App.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string DefaultConnection = "DefaultConnection";

        public UnitOfWork(
            IAdjuntoTipoRepository adjuntoTipos,
            IAliveCheckEstadoRepository aliveCheckEstados,
            IAliveCheckRepository aliveChecks,
            IAliveCheckRespuestaRepository aliveCheckRespuestas,
            IAsignacionEventoRepository asignacionEventos,
            IAsignacionEventoTipoRepository asignacionEventoTipos,
            IAsignacionRepository asignaciones,
            IClienteRepository clientes,
            IClienteUsuarioRepository clienteUsuarios,
            IConfiguracionRepository configuraciones,
            IControlPointRepository controlPoints,
            IEventoAdjuntoRepository eventoAdjuntos,
            IEventoRepository eventos,
            IEventoTipoRepository eventoTipos,
            IPanicAlertAdjuntoRepository panicAlertAdjuntos,
            IPanicAlertEstadoRepository panicAlertEstados,
            IPanicAlertNotificacionRepository panicAlertNotificaciones,
            IPanicAlertRepository panicAlerts,
            IPuestoRepository puestos,
            IRolRepository roles,
            IRondaAdjuntoRepository rondaAdjuntos,
            IRondaRepository rondas,
            ISesionUsuarioEvidenciaRepository sesionUsuarioEvidencias,
            ISesionUsuarioRepository sesionUsuarios,
            ITipoConfiguracionRepository tipoConfiguraciones,
            ITipoDocumentoRepository tipoDocumento,
            ITurnoRepository turnos,
            IUnidadRepository unidades,
            IUsuarioEstadoRepository usuarioEstados,
            IUsuarioPuestoRepository usuarioPuestos,
            IUsuarioRepository usuarios,
            IUsuarioRolRepository usuarioRoles,
            IUsuarioUnidadRepository usuarioUnidades)
        {
            AdjuntoTipos = adjuntoTipos;
            AliveCheckEstados = aliveCheckEstados;
            AliveChecks = aliveChecks;
            AliveCheckRespuestas = aliveCheckRespuestas;
            AsignacionEventos = asignacionEventos;
            AsignacionEventoTipos = asignacionEventoTipos;
            Asignaciones = asignaciones;
            Clientes = clientes;
            ClienteUsuarios = clienteUsuarios;
            Configuraciones = configuraciones;
            ControlPoints = controlPoints;
            EventoAdjuntos = eventoAdjuntos;
            Eventos = eventos;
            EventoTipos = eventoTipos;
            PanicAlertAdjuntos = panicAlertAdjuntos;
            PanicAlertEstados = panicAlertEstados;
            PanicAlertNotificaciones = panicAlertNotificaciones;
            PanicAlerts = panicAlerts;
            Puestos = puestos;
            Roles = roles;
            RondaAdjuntos = rondaAdjuntos;
            Rondas = rondas;
            SesionUsuarioEvidencias = sesionUsuarioEvidencias;
            SesionUsuarios = sesionUsuarios;
            TipoConfiguraciones = tipoConfiguraciones;
            TipoDocumento = tipoDocumento;
            Turnos = turnos;
            Unidades = unidades;
            UsuarioEstados = usuarioEstados;
            UsuarioPuestos = usuarioPuestos;
            Usuarios = usuarios;
            UsuarioRoles = usuarioRoles;
            UsuarioUnidades = usuarioUnidades;
        }

        public IAdjuntoTipoRepository AdjuntoTipos { get; }

        public IAliveCheckEstadoRepository AliveCheckEstados { get; }

        public IAliveCheckRepository AliveChecks { get; }

        public IAliveCheckRespuestaRepository AliveCheckRespuestas { get; }

        public IAsignacionEventoRepository AsignacionEventos { get; }

        public IAsignacionEventoTipoRepository AsignacionEventoTipos { get; }

        public IAsignacionRepository Asignaciones { get; }

        public IClienteRepository Clientes { get; }

        public IClienteUsuarioRepository ClienteUsuarios { get; }

        public IConfiguracionRepository Configuraciones { get; }

        public IControlPointRepository ControlPoints { get; }

        public IEventoAdjuntoRepository EventoAdjuntos { get; }

        public IEventoRepository Eventos { get; }

        public IEventoTipoRepository EventoTipos { get; }

        public IPanicAlertAdjuntoRepository PanicAlertAdjuntos { get; }

        public IPanicAlertEstadoRepository PanicAlertEstados { get; }

        public IPanicAlertNotificacionRepository PanicAlertNotificaciones { get; }

        public IPanicAlertRepository PanicAlerts { get; }

        public IPuestoRepository Puestos { get; }

        public IRolRepository Roles { get; }

        public IRondaAdjuntoRepository RondaAdjuntos { get; }

        public IRondaRepository Rondas { get; }

        public ISesionUsuarioEvidenciaRepository SesionUsuarioEvidencias { get; }

        public ISesionUsuarioRepository SesionUsuarios { get; }

        public ITipoConfiguracionRepository TipoConfiguraciones { get; }

        public ITipoDocumentoRepository TipoDocumento { get; }

        public ITurnoRepository Turnos { get; }

        public IUnidadRepository Unidades { get; }

        public IUsuarioEstadoRepository UsuarioEstados { get; }

        public IUsuarioPuestoRepository UsuarioPuestos { get; }

        public IUsuarioRepository Usuarios { get; }

        public IUsuarioRolRepository UsuarioRoles { get; }

        public IUsuarioUnidadRepository UsuarioUnidades { get; }
    }
}
