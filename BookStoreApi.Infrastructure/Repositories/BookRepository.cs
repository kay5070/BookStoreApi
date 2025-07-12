using BookStoreApi.Domain.Entities;
using BookStoreApi.Application.Interfaces;
using BookStoreApi.Data;

namespace BookStoreApi.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Book> GetAll()
    {
        return _context.Books.ToList();
    }

    public Book? GetById(int id)
    {
        return _context.Books.Find(id);
    }

    public void Add(Book book)
    {
        _context.Books.Add(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}