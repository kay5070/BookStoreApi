using BookStoreApi.Data;
using BookStoreApi.Dtos;
using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController:ControllerBase
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<BookReadDto>> GetBooks()
    {
        var books = _context.Books.Select(b=>new BookReadDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Year = b.Year,
            Price = b.Price
        })    
            .ToList();
        return Ok(books);
    }

    [HttpGet("id")]
    public ActionResult<BookReadDto> GetBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) 
            return NotFound();
        var dto = new BookReadDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price
        };
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<BookReadDto> CreateBook(BookCreateDto BookDto)
    {
        var book = new Book
        {
            Title = BookDto.Title,
            Author = BookDto.Author,
            Year = BookDto.Year,
            Price = BookDto.Price
        };
        _context.Books.Add(book);
        _context.SaveChanges();
        var readDto = new BookReadDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price
        };
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, readDto);
    }

    [HttpPut]
    public IActionResult UpdateBook(int id, BookCreateDto bookDto)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        book.Title = bookDto.Title;
        book.Author = bookDto.Author;
        book.Year = bookDto.Year;
        book.Price = bookDto.Price;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        _context.Books.Remove(book);
        _context.SaveChanges();
        return NoContent();
    }
}
























