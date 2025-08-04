using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Dtos;

public class BookPatchDto
{
    public string? Title { get; set; }

    public string? Author { get; set; }

    public int? Year { get; set; }

    public decimal? Price { get; set; }
}