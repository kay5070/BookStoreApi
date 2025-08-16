using System.Net;
using BookStoreApi.Application.Dtos;
using BookStoreApi.Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorController:ControllerBase
{
    private readonly IAuthorsService _service;

    public AuthorController(IAuthorsService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorReadDto>>> GetAuthors()
    {
        var authors = await _service.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
    {
        var author = await _service.GetByIdAsync(id);
        return Ok(author);
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthorReadDto>> CreateAuthor(AuthorCreateDto authorDto)
    {
        return await _service.CreateAsync(authorDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto authorDto)
    {
        return await _service.UpdateAsync(id, authorDto) ? NoContent() : NotFound();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        return await _service.DeleteAsync(id) ? NoContent():NotFound();
    }
}