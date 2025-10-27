namespace App.WebApi.Entities
{
    public class AreaDto
    {
        public required string nombre { get; set; }
        public List<Coordenada>? coordenadas { get; set; }
    }
}
