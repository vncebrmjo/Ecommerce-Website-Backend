namespace Ecommerce_Website_Backend.Models.Response
{
    public record LoginResponse(
        string Token,
        string FirstName,
        string LastName,
        string Email,
        string UserName,
        string Role,
        DateTime ExpiresAt);
}
