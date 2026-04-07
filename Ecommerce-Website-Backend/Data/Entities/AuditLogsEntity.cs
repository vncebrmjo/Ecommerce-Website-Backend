namespace Ecommerce_Website_Backend.Data.Entities
{
    public class AuditLogsEntity : BaseEntity
    {
        public string EntityName { get; set; } = string.Empty; 
        public string EntityId { get; set; } = string.Empty;    
        public string Action { get; set; } = string.Empty;      
        public string? OldValues { get; set; }                  
        public string? NewValues { get; set; }  
        
        // To be implemented when the login is finished
        public string? ChangedBy { get; set; }            
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    }
}
