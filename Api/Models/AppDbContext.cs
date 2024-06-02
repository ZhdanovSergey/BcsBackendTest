using Microsoft.EntityFrameworkCore;

namespace BcsBackendTest.Models;

sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Message> Messages { get; init; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
