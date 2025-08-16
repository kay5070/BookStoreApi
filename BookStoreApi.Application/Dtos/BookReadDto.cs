namespace BookStoreApi.Application.Dtos;

public class BookReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public int AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
    
    public DateOnly PublishDate { get; set; }
    public decimal Price { get; set; }
}