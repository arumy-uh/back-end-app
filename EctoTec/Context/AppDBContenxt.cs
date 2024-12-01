using EctoTec.Models;
using Microsoft.EntityFrameworkCore;

namespace EctoTec.Context
{
    public class AppDBContenxt : DbContext
    {
        public AppDBContenxt(DbContextOptions<AppDBContenxt> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
