using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;

namespace App.Infrastructure.Repository.Core
{
    public class TipoConfiguracionRepository : ITipoConfiguracionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public TipoConfiguracionRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IReadOnlyList<TipoConfiguracion>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<TipoConfiguracion?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
