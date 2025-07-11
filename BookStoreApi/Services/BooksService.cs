using AutoMapper;
using BookStoreApi.Data;
using BookStoreApi.Dtos;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Services;

public class BooksService : IBooksService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BooksService(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public IEnumerable<BookReadDto> GetAll()
    {
        var books = _repository.GetAll();
        return _mapper.Map<IEnumerable<BookReadDto>>(books);
    }

    public BookReadDto? GetById(int id)
    {
        var book = _repository.GetById(id);
        return book == null ? null : _mapper.Map<BookReadDto>(book);
    }

    public BookReadDto Create(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _repository.Add(book);
        _repository.Save();
        return _mapper.Map<BookReadDto>(book);
    }

    public bool Update(int id, BookUpdateDto bookDto)
    {

        var book = _repository.GetById(id);
        if (book == null) return false;

        _mapper.Map(bookDto, book);
        _repository.Save();
        return true;
    }

    public bool PatchBook(int id, JsonPatchDocument<BookPatchDto> patchDoc, ModelStateDictionary modelState)
    {
        var book = _repository.GetById(id);
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
        _repository.Save();
        return true;
    }

    public bool Delete(int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return false;

       _repository.Delete(book);
       _repository.Save();
        return true;
    }
}