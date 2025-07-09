using System.Xml.XPath;
using AutoMapper;
using BookStoreApi.Data;
using BookStoreApi.Dtos;
using BookStoreApi.Models;
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

    public BookController(AppDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<BookReadDto>> GetBooks()
    {
        var books = _context.Books.ToList();
        var bookDtos = _mapper.Map<IEnumerable<BookReadDto>>(books);
        return Ok(bookDtos);
    }

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BookReadDto> GetBookById(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) 
            return NotFound();
        var bookDto = _mapper.Map<BookReadDto>(book);
        return Ok(bookDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(BookReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<BookReadDto> CreateBook(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _context.Books.Add(book);
        _context.SaveChanges();
        
        var bookReadDto = _mapper.Map<BookReadDto>(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, bookReadDto);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PatchBook(int id, JsonPatchDocument<BookPatchDto>? patchDoc)
    {
        if(patchDoc == null)
            return BadRequest("Patch document cannot be null.");
        var book = _context.Books.FirstOrDefault(b=>b.Id==id);
        if(book == null)
            return NotFound();
        
        //Map entity to patchable DTO
        var bookToPatch =  _mapper.Map<BookPatchDto>(book);
        // apply patch
        patchDoc.ApplyTo(bookToPatch,ModelState);
        
        //validate patched dto
        
        if(!TryValidateModel(bookToPatch))
            return BadRequest(ModelState);
        
        //map patched dto back to entity
        _mapper.Map(bookToPatch, book);
        
        _context.SaveChanges();
        return NoContent();        
    }
    [HttpPut]
    public IActionResult UpdateBook(int id, BookCreateDto bookDto)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        _mapper.Map(bookDto, book);
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
























