using App.Core.Entities.Core;

namespace App.Application.Interfaces.Core
{
    // Repositorio para `usuario` (operaciones completas), clave Guid
    public interface IUsuarioRepository : IGenericAutoIdRepository<Usuario, Guid>
    {
        /// <summary>
        /// Obtener usuario por su nombre de usuario (login).
        /// </summary>
        Task<Usuario?> GetByNameAsync(string nombreUsuario);

        /// <summary>
        /// Valida que la contraseña plana `clave` coincida con el hash almacenado para el usuario.
        /// Implementación típica: recuperar el hash y usar la utilidad de hashing/verificación.
        /// Devuelve true si la clave es válida.
        /// </summary>
        Task<bool> ValidatePasswordAsync(Guid usuarioId, string clave);

        /// <summary>
        /// Actualiza la contraseña del usuario (recibe la clave plana).
        /// Implementación típica: generar hash, guardar en base de datos y actualizar sello_seguridad si procede.
        /// </summary>
        Task UpdatePasswordAsync(Guid usuarioId, string clave);
    }
}
