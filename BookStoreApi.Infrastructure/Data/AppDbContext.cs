using BookStoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Data;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Book> Books => Set<Book>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasPrecision(18, 2);
    }
}