namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia
    /// Registros de ocurrencias asociadas a sesiones y puestos.
    /// </summary>
    public class Evento : AuditableEntity<Guid>
    {
        public Guid? sesion_usuario_id { get; set; }
        public Guid? ronda_id { get; set; }
        public Guid? puesto_id { get; set; }
        public Guid tipo_evento_id { get; set; }
        public string? descripcion { get; set; }
        public DateTimeOffset fecha_hora { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public int? turno_id { get; set; }
    }
}
