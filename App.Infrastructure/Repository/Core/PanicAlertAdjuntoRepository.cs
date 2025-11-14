using App.Application.Interfaces;
using App.Application.Interfaces.Core;
using App.Core.Entities.Core;
using App.Infrastructure.Database;
using Dapper;

namespace App.Infrastructure.Repository.Core
{
    public class PanicAlertAdjuntoRepository : IPanicAlertAdjuntoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IFileStorageService _fileStorage;

        public PanicAlertAdjuntoRepository(IDbConnectionFactory dbFactory, IFileStorageService fileStorage)
        {
            _dbFactory = dbFactory;
            _fileStorage = fileStorage;
        }

        public async Task<Guid> AddAsync(Guid createdBy, Guid panicAlertId, Stream content, string originalFileName, CancellationToken cancellationToken = default)
        {
            // Save file to storage
            var relativePath = await _fileStorage.SaveAsync(content, originalFileName, "PanicAlertAdjunto", cancellationToken);

            // Determine adjunto_tipo_id based on file extension
            string adjuntoTipoId = GetTipoIdFromFileName(originalFileName);

            const string sql = "INSERT INTO panic_alert_adjunto (panic_alert_id, adjunto_tipo_id, ruta, created_at, created_by) VALUES (@panic_alert_id, @adjunto_tipo_id, @ruta, @created_at, @created_by) RETURNING id";

            var p = new DynamicParameters();
            p.Add("@panic_alert_id", panicAlertId);
            p.Add("@adjunto_tipo_id", adjuntoTipoId);
            p.Add("@ruta", relativePath);
            p.Add("@created_at", DateTimeOffset.UtcNow);
            p.Add("@created_by", createdBy);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                using var tx = connection.BeginTransaction();
                try
                {
                    var id = await connection.ExecuteScalarAsync<Guid>(sql, p, tx);
                    tx.Commit();
                    return id;
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }

        private static string GetTipoIdFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return "03"; // texto por defecto
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext)) return "03";

            // Image extensions
            var imageExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff" };
            if (imageExt.Contains(ext)) return "01";

            // Audio extensions
            var audioExt = new[] { ".mp3", ".wav", ".m4a", ".aac", ".ogg", ".flac" };
            if (audioExt.Contains(ext)) return "02";

            // Default to texto
            return "03";
        }

        public async Task<IReadOnlyList<PanicAlertAdjunto>> GetAllAsync(Guid panicAlertId)
        {
            const string sql = "SELECT * FROM panic_alert_adjunto WHERE panic_alert_id = @panicAlertId AND deleted_at IS NULL ORDER BY created_at";

            var p = new DynamicParameters();
            p.Add("@panicAlertId", panicAlertId);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                var items = await connection.QueryAsync<PanicAlertAdjunto>(sql, p);
                return items.AsList();
            }
        }

        public async Task<PanicAlertAdjunto?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM panic_alert_adjunto WHERE id = @id AND deleted_at IS NULL";

            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<PanicAlertAdjunto>(sql, p);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "UPDATE panic_alert_adjunto SET deleted_at = now() WHERE id = @id";
            var p = new DynamicParameters();
            p.Add("@id", id);

            using (var connection = _dbFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, p);
            }
        }
    }
}
