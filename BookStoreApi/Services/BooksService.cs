using AutoMapper;
using BookStoreApi.Data;
using BookStoreApi.Dtos;
using BookStoreApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Services;

public class BooksService : IBooksService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BooksService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public IEnumerable<BookReadDto> GetAll()
    {
        var books = _context.Books.ToList();
        return _mapper.Map<IEnumerable<BookReadDto>>(books);
    }

    public BookReadDto? GetById(int id)
    {
        var book = _context.Books.Find(id);
        return book == null ? null : _mapper.Map<BookReadDto>(book);
    }

    public BookReadDto Create(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _context.Books.Add(book);
        _context.SaveChanges();
        return _mapper.Map<BookReadDto>(book);
    }

    public bool Update(int id, BookUpdateDto bookDto)
    {

        var book = _context.Books.Find(id);
        if (book == null) return false;

        _mapper.Map(bookDto, book);
        _context.SaveChanges();
        return true;
    }

    public bool PatchBook(int id, JsonPatchDocument<BookPatchDto> patchDoc, ModelStateDictionary modelState)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return false;
        
        // 1. Map book to patchable DTO
        var bookToPatch = _mapper.Map<BookPatchDto>(book);
        
        //2. Apply patch to DTO
        patchDoc.ApplyTo(bookToPatch,modelState);
        
        //3. Validate
        if (!modelState.IsValid)
            return false;
        
        //4. Map patched DTO back to entity
        _mapper.Map(bookToPatch, book);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }
}