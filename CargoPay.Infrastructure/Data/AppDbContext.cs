using CargoPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(c => c.Id);
                
                entity.Property(c => c.CardNumber)
                      .HasMaxLength(15)
                      .IsRequired();

                entity.Property(c => c.Balance)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();
            });
        }
    }
}
