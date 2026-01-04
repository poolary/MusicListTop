using CRUD.Models;
using Microsoft.EntityFrameworkCore;


namespace CRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Musica> MusicasLike { get; set; }
        public DbSet<User> UsersPW { get; set; }
    }
}
