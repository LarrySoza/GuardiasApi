using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para la tabla usuario_rol (tiene auditoría)
    public interface IUsuarioRolRepository
    {
        Task AddAsync(Guid usuario_id, string rol_id);
        Task DeleteAsync(Guid usuario_id, string rol_id);
        Task<IReadOnlyList<Rol>> GetAllAsync(Guid usuario_id);
    }
}
