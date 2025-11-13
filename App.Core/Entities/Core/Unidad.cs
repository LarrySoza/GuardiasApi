namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: unit
    /// Units or locations belonging to a client. Allows hierarchy.
    /// </summary>
    public class Unidad : AuditableEntity<Guid>
    {
        public Guid? cliente_id { get; private set; }
        public Guid? unidad_id_padre { get; private set; }
        public string? nombre { get; private set; }
        public string? direccion { get; private set; }

        // Permite cambiar el padre de la unidad respetando la encapsulación
        public void SetParent(Guid? parentId)
        {
            unidad_id_padre = parentId;
        }
    }
}
