using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Application.Interfaces;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    void Add(Book book);
    void Update(Book book);
    void Delete(Book book);
    void Save();
}