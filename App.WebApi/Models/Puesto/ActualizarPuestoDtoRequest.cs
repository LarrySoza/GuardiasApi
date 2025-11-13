namespace App.WebApi.Models.Puesto
{
    public class ActualizarPuestoDtoRequest
    {
        public string? nombre { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }

        /// <summary>
        /// Lista de ids de turnos a asociar al puesto. Si es null, no se realizan cambios en turnos.
        /// Si se envía lista vacía, se eliminarán todas las asociaciones.
        /// </summary>
        public List<int>? turnos_ids { get; set; }
    }
}
