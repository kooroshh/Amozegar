using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data
{
    public class AmozegarContext : DbContext
    {
        public AmozegarContext(DbContextOptions<AmozegarContext> option) : base(option){}

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Report>()
                .Property(r => r.Date)
                .HasDefaultValueSql("GETDATE()");
        }

    }
}
