using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Domain.Entities;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly PublishDate { get; set; }
    
    public Author Author { get; set; }
    public int AuthorId { get; set; }
}