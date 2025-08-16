using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Application.Dtos;

public class AuthorReadDto
{
    public int  Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public ICollection<BookBriefDto> Books { get; set; } = new List<BookBriefDto>();
}