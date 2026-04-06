namespace Ecommerce_Website_Backend.Models.Response
{
    public sealed record ApiErrorResponse
    {
        public required int Status { get; init; }
        public required string Title { get; init; }
        public string? Detail { get; init; }
        public Dictionary<string, string[]>? Errors { get; init; }
    }
}
