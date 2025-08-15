using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Application.Dtos;

public class BookUpdateDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "AuthorId is required")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Publish Data is required")]
    public DateOnly PublishDate { get; set; }
    
    [Range(1,1000,ErrorMessage = "Range is between 1 and 1000")]
    public decimal Price { get; set; }
}