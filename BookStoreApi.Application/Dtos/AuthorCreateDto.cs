using System.ComponentModel.DataAnnotations;
using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Application.Dtos;

public class AuthorCreateDto
{   [Required(ErrorMessage = "FirstName is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "LastName is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string LastName { get; set; } = string.Empty;

}