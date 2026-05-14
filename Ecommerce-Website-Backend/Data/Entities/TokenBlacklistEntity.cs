namespace Ecommerce_Website_Backend.Data.Entities
{
    public class TokenBlacklistEntity : BaseEntity
    {
        public string SessionToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
