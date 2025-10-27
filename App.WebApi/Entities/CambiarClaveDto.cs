namespace App.WebApi.Entities
{
    public class CambiarClaveDto
    {
        public required string ClaveActual { get; set; }
        public required string ClaveNueva { get; set; }
    }
}
