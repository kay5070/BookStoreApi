using BookStoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Author> Authors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed Authors
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
            new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling" },
            new Author { Id = 3, FirstName = "Jane", LastName = "Austen" },
            new Author { Id = 4, FirstName = "Mark", LastName = "Twain" },
            new Author { Id = 5, FirstName = "Fyodor", LastName = "Dostoevsky" }
        );

        // Seed Books
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "1984",
                Price = 15.99m,
                PublishDate = new DateOnly(1949, 6, 8),
                AuthorId = 1
            },
            new Book
            {
                Id = 2,
                Title = "Harry Potter and the Sorcerer's Stone",
                Price = 19.99m,
                PublishDate = new DateOnly(1997, 6, 26),
                AuthorId = 2
            },
            new Book
            {
                Id = 3,
                Title = "Pride and Prejudice",
                Price = 12.5m,
                PublishDate = new DateOnly(1813, 1, 28),
                AuthorId = 3
            },
            new Book
            {
                Id = 4,
                Title = "Adventures of Huckleberry Finn",
                Price = 14.2m,
                PublishDate = new DateOnly(1884, 12, 10),
                AuthorId = 4
            },
            new Book
            {
                Id = 5,
                Title = "Crime and Punishment",
                Price = 17.75m,
                PublishDate = new DateOnly(1866, 1, 1),
                AuthorId = 5
            }
        );
    }
}
    