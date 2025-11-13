using App.WebApi.Models.Common;

namespace App.WebApi.Models.Unidad
{
    /// <summary>
    /// DTO que representa una Unidad y sus datos de auditoría.
    /// </summary>
    public class UnidadDto : AuditableEntityDto
    {
        /// <summary>
        /// Identificador único de la unidad.
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Identificador del cliente asociado.
        /// </summary>
        public Guid? cliente_id { get; set; }
        /// <summary>
        /// Identificador de la unidad padre en la jerarquía.
        /// </summary>
        public Guid? unidad_id_padre { get; set; }
        /// <summary>
        /// Nombre de la unidad.
        /// </summary>
        public string? nombre { get; set; }
        /// <summary>
        /// Dirección de la unidad.
        /// </summary>
        public string? direccion { get; set; }

        // hijos para representar el árbol
        public List<UnidadDto> children { get; set; } = new();
    }
}