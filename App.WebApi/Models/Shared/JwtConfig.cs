namespace App.WebApi.Models.Shared
{
    public class JwtConfig
    {
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int LifetimeSeconds { get; set; }
    }
}
