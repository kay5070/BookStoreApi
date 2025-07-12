using AutoMapper;
using BookStoreApi.Application.Interfaces;
using BookStoreApi.Domain.Entities;
using BookStoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Application.Services;

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
    public BookPatchDto? GetByIdForPatch(int id)
    {
        var book = _repository.GetById(id);
        return book == null ? null : _mapper.Map<BookPatchDto>(book);
    }

    public bool ApplyPatch(int id, BookPatchDto patchedDto)
    {
        var book = _repository.GetById(id);
        if (book == null)
            return false;

        _mapper.Map(patchedDto, book);
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