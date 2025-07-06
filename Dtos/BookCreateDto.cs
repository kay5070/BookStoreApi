using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Dtos;

public class BookCreateDto
{   
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Author { get; set; } = string.Empty;

    [Range(1500,2100)]
    public int Year { get; set; }

    [Range(0,1000)]
    public decimal Price { get; set; }
}