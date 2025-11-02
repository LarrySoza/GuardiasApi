using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para usuario_unidad (tiene auditoría)
    public interface IUsuarioUnidadRepository : IGenericAutoIdRepository<UsuarioUnidad, (Guid usuario_id, Guid unidad_id)>
    {
    }
}
