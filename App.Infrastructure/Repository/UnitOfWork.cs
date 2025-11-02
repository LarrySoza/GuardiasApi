using App.Application.Interfaces;
using App.Application.Interfaces.Core;

namespace App.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public const string DefaultConnection = "DefaultConnection";

        public UnitOfWork(
            IAliveCheckEstadoRepository aliveCheckEstados,
            IAliveCheckRepository aliveChecks,
            IAliveCheckRespuestaRepository aliveCheckRespuestas,
            IAsignacionEventoRepository asignacionEventos,
            IAsignacionEventoTipoRepository asignacionEventoTipos,
            IAsignacionRepository asignaciones,
            IClienteUsuarioRepository clienteUsuarios,
            IClienteRepository clientes,
            IConfiguracionRepository configuraciones,
            IControlPointRepository controlPoints,
            IIncidenciaAdjuntoRepository incidenciaAdjuntos,
            IIncidenciaRepository incidencias,
            IIncidenteTipoRepository incidenteTipos,
            IOcurrenciaAdjuntoRepository ocurrenciaAdjuntos,
            IOcurrenciaRepository ocurrencias,
            IOcurrenciaTipoRepository ocurrenciaTipos,
            IPanicAlertAdjuntoTipoRepository panicAlertAdjuntoTipos,
            IPanicAlertAdjuntoRepository panicAlertAdjuntos,
            IPanicAlertEstadoRepository panicAlertEstados,
            IPanicAlertRepository panicAlerts,
            IPanicAlertRecepcionRepository panicAlertRecepciones,
            IPuestoRepository puestos,
            IRondaAdjuntoRepository rondaAdjuntos,
            IRondaRepository rondas,
            IRolRepository roles,
            ISesionUsuarioEvidenciaRepository sesionUsuarioEvidencias,
            ISesionUsuarioRepository sesionUsuarios,
            ITipoConfiguracionRepository tipoConfiguraciones,
            IUnidadRepository unidades,
            IUsuarioEstadoRepository usuarioEstados,
            IUsuarioRolRepository usuarioRoles,
            IUsuarioUnidadRepository usuarioUnidades,
            IUsuarioRepository usuarios)
        {
            AliveCheckEstados = aliveCheckEstados;
            AliveChecks = aliveChecks;
            AliveCheckRespuestas = aliveCheckRespuestas;
            AsignacionEventos = asignacionEventos;
            AsignacionEventoTipos = asignacionEventoTipos;
            Asignaciones = asignaciones;
            ClienteUsuarios = clienteUsuarios;
            Clientes = clientes;
            Configuraciones = configuraciones;
            ControlPoints = controlPoints;
            IncidenciaAdjuntos = incidenciaAdjuntos;
            Incidencias = incidencias;
            IncidenteTipos = incidenteTipos;
            OcurrenciaAdjuntos = ocurrenciaAdjuntos;
            Ocurrencias = ocurrencias;
            OcurrenciaTipos = ocurrenciaTipos;
            PanicAlertAdjuntoTipos = panicAlertAdjuntoTipos;
            PanicAlertAdjuntos = panicAlertAdjuntos;
            PanicAlertEstados = panicAlertEstados;
            PanicAlerts = panicAlerts;
            PanicAlertRecepciones = panicAlertRecepciones;
            Puestos = puestos;
            RondaAdjuntos = rondaAdjuntos;
            Rondas = rondas;
            Roles = roles;
            SesionUsuarioEvidencias = sesionUsuarioEvidencias;
            SesionUsuarios = sesionUsuarios;
            TipoConfiguraciones = tipoConfiguraciones;
            Unidades = unidades;
            UsuarioEstados = usuarioEstados;
            UsuarioRoles = usuarioRoles;
            UsuarioUnidades = usuarioUnidades;
            Usuarios = usuarios;
        }

        public IAliveCheckEstadoRepository AliveCheckEstados { get; }

        public IAliveCheckRepository AliveChecks { get; }

        public IAliveCheckRespuestaRepository AliveCheckRespuestas { get; }

        public IAsignacionEventoRepository AsignacionEventos { get; }

        public IAsignacionEventoTipoRepository AsignacionEventoTipos { get; }

        public IAsignacionRepository Asignaciones { get; }

        public IClienteUsuarioRepository ClienteUsuarios { get; }

        public IClienteRepository Clientes { get; }

        public IConfiguracionRepository Configuraciones { get; }

        public IControlPointRepository ControlPoints { get; }

        public IIncidenciaAdjuntoRepository IncidenciaAdjuntos { get; }

        public IIncidenciaRepository Incidencias { get; }

        public IIncidenteTipoRepository IncidenteTipos { get; }

        public IOcurrenciaAdjuntoRepository OcurrenciaAdjuntos { get; }

        public IOcurrenciaRepository Ocurrencias { get; }

        public IOcurrenciaTipoRepository OcurrenciaTipos { get; }

        public IPanicAlertAdjuntoTipoRepository PanicAlertAdjuntoTipos { get; }

        public IPanicAlertAdjuntoRepository PanicAlertAdjuntos { get; }

        public IPanicAlertEstadoRepository PanicAlertEstados { get; }

        public IPanicAlertRepository PanicAlerts { get; }

        public IPanicAlertRecepcionRepository PanicAlertRecepciones { get; }

        public IPuestoRepository Puestos { get; }

        public IRondaAdjuntoRepository RondaAdjuntos { get; }

        public IRondaRepository Rondas { get; }

        public IRolRepository Roles { get; }

        public ISesionUsuarioEvidenciaRepository SesionUsuarioEvidencias { get; }

        public ISesionUsuarioRepository SesionUsuarios { get; }

        public ITipoConfiguracionRepository TipoConfiguraciones { get; }

        public IUnidadRepository Unidades { get; }

        public IUsuarioEstadoRepository UsuarioEstados { get; }

        public IUsuarioRolRepository UsuarioRoles { get; }

        public IUsuarioUnidadRepository UsuarioUnidades { get; }

        public IUsuarioRepository Usuarios { get; }
    }
}
