namespace App.Core.Entities
{
    // Representa una entidad base con una propiedad `id` (coincide con la columna SQL `id`)
    public abstract class Entity<TId>
    {
        public TId? id { get; protected set; }

        // Establece los campos de auditoría de actualización
        public void SetId(TId id)
        {
            this.id = id;
        }
    }
}
