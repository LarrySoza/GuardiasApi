using App.Application.Interfaces.Core;
using App.Core.Entities;
using App.Core.Entities.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoTipoRepository : IPanicAlertAdjuntoTipoRepository
    {
        private readonly IConfiguration _config;

        public PanicAlertAdjuntoTipoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IReadOnlyList<PanicAlertAdjuntoTipo>> GetAllAsync()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task<PanicAlertAdjuntoTipo?> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
