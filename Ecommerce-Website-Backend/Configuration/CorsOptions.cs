namespace Ecommerce_Website_Backend.Configuration
{
    public sealed class CorsOptions
    {
        public const string SectionName = "Cors";

        public required string[] AllowedOrigins { get; set; }
        public required string PolicyName { get; set; }
    }
}
