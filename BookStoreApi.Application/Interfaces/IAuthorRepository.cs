using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Application.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task AddAsync(Author author);
    void Update(Author author);
    void Delete(Author author);
    Task SaveAsync();
}