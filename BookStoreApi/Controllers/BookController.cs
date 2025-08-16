using BookStoreApi.Application.Dtos;
using BookStoreApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BookController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookReadDto>>> GetBooks()
    {
        var books = await _booksService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookReadDto>> GetBookById(int id)
    {
        var book = await _booksService.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookReadDto>> CreateBook(BookCreateDto bookDto)
    {
        var createdBook = await _booksService.CreateAsync(bookDto);
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, BookUpdateDto bookDto)
    {
        return await _booksService.UpdateAsync(id, bookDto) ? NoContent() : NotFound();
    }
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        return await _booksService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}