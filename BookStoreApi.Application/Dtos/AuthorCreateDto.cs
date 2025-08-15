using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Application.Dtos;

public class AuthorCreateDto
{
    [Required] [MaxLength(100)] public string FullName { get; set; } = string.Empty;
}