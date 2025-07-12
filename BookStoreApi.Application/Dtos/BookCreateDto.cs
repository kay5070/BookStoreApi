using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Dtos;

public class BookCreateDto
{   
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Author is required")]
    [MaxLength(50, ErrorMessage = "Max length is 50")]
    public string Author { get; set; } = string.Empty;

    [Range(1500,2100,ErrorMessage = "Range is between 1500 and 2100")]
    public int Year { get; set; }

    [Range(1,1000,ErrorMessage = "Range is between 1 and 1000")]
    public decimal Price { get; set; }
}