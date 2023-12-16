using Microsoft.EntityFrameworkCore;
using NotificationService.Repository.Models;

namespace NotificationService.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InfoTable>().ToTable("Info");
            modelBuilder.Entity<InfoTable>().HasKey(x => x.IDInfo);
            modelBuilder.Entity<InfoTable>().Property(e => e.IDInfo).ValueGeneratedOnAdd();

            modelBuilder.Entity<WarningTable>().ToTable("Warning");
            modelBuilder.Entity<WarningTable>().HasKey(x => x.IDWarning);
            modelBuilder.Entity<WarningTable>().Property(e => e.IDWarning).ValueGeneratedOnAdd();

            modelBuilder.Entity<CriticalTable>().ToTable("Critical");
            modelBuilder.Entity<CriticalTable>().HasKey(x => x.IDCritical);
            modelBuilder.Entity<CriticalTable>().Property(e => e.IDCritical).ValueGeneratedOnAdd();
        }

        public DbSet<InfoTable> Info { get; set; }
        public DbSet<WarningTable> Warning { get; set; }
        public DbSet<CriticalTable> Critical { get; set; }
    }
}
