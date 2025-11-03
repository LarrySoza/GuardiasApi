namespace App.WebApi.Models.Unidad
{
    /// <summary>
    /// DTO para actualizar el padre de una unidad (unidad_id_padre).
    /// </summary>
    public class ParentUpdateDto
    {
        public Guid? parentId { get; set; }
    }
}
