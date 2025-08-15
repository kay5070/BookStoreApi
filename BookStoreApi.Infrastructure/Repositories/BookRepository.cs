using BookStoreApi.Application.Interfaces;
using BookStoreApi.Domain.Entities;
using BookStoreApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.Include(b=>b.Author).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id==id);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}