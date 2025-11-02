namespace App.Core.Entities
{
    /// <summary>
    /// ValueObject for the jsonb field `equipos_a_cargo` in `sesion_usuario`.
    /// </summary>
    public class EquiposACargo
    {
        public List<string> Items { get; set; } = new();
    }
}
