using Microsoft.EntityFrameworkCore;

namespace Qorpe.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
}
