using BookStoreApi.Application.Dtos;

namespace BookStoreApi.Application.Interfaces;

public interface IAuthorsService
{
    Task<IEnumerable<AuthorReadDto>> GetAllAsync();
    Task<AuthorReadDto?> GetByIdAsync(int id);
    Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto);
    Task<bool> DeleteAsync(int id);
}