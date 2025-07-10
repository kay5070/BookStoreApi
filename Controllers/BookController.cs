using System.Xml.XPath;
using AutoMapper;
using BookStoreApi.Data;
using BookStoreApi.Dtos;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookController:ControllerBase
{
    
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBooksService _booksService;

    public BookController(AppDbContext context,IMapper mapper,IBooksService booksService)
    {
        _context = context;
        _mapper = mapper;
        _booksService = booksService;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<BookReadDto>> GetBooks()
    {
        var books = _booksService.GetAll();
        return Ok(books);
    }

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BookReadDto> GetBookById(int id)
    {
        var book = _booksService.GetById(id);
        if (book == null) 
            return NotFound();
        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(BookReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<BookReadDto> CreateBook(BookCreateDto bookDto)
    {
        var createdBook  = _booksService.Create(bookDto);
        
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PatchBook(int id, JsonPatchDocument<BookPatchDto>? patchDoc)
    {
        if (patchDoc == null)
            return BadRequest("Patch document cannot  be null.");

        var result = _booksService.PatchBook(id, patchDoc, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return result ? NoContent() : NotFound();

    }
    [HttpPut]
    public IActionResult UpdateBook(int id, BookUpdateDto  bookDto)
    {
        return _booksService.Update(id, bookDto) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        return _booksService.Delete(id) ? NoContent() : NotFound();
    }
}
























