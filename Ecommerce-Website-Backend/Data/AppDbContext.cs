using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
    }
}
