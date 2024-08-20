using Microsoft.EntityFrameworkCore;
using EFConsoleApp.Models;

namespace EFConsoleApp.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MyDatabase;User Id=sa;Password=P@ssw0rd123;TrustServerCertificate=True;");
        }
    }
}
