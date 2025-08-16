using BookStoreApi.Application.Interfaces;
using BookStoreApi.Domain.Entities;
using BookStoreApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Infrastructure.Repositories;

public class AuthorRepository:IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(a=>a.Books).ToListAsync();
        }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.Include(a=>a.Books).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Author author)
    { 
        await _context.Authors.AddAsync(author);
    }

    public void Update(Author author)
    {
        _context.Authors.Update(author);
    }

    public void Delete(Author author)
    {
        _context.Authors.Remove(author);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}