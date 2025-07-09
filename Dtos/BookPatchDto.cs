using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Dtos;

public class BookPatchDto
{
    [StringLength(100)]
    public string? Title { get; set; }

    [StringLength(100)]
    public string? Author { get; set; }

    [Range(1500, 2100)]
    public int? Year { get; set; }

    [Range(0.01, 1000.00)]
    public decimal? Price { get; set; }
}