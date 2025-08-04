using AutoMapper;
using BookStoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BookStoreApi.Domain.Entities;

public class BooksService : IBooksService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BooksService(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookReadDto>> GetAllAsync()
    {
        var books = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookReadDto>>(books);
    }

    public async Task<BookReadDto?> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        return book == null ? null : _mapper.Map<BookReadDto>(book);
    }

    public async Task<BookReadDto> CreateAsync(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        await _repository.AddAsync(book);
        await _repository.SaveAsync();
        return _mapper.Map<BookReadDto>(book);
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto bookDto)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return false;

        _mapper.Map(bookDto, book);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> PatchBookAsync(int id, JsonPatchDocument<BookPatchDto> patchDoc, ModelStateDictionary modelState)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null)
            return false;

        var bookToPatch = _mapper.Map<BookPatchDto>(book);

        patchDoc.ApplyTo(bookToPatch, error =>
        {
            modelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
        });



        if (!modelState.IsValid)
            return false;

        _mapper.Map(bookToPatch, book);
        await _repository.SaveAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return false;

        _repository.Delete(book);
        await _repository.SaveAsync();
        return true;
    }
}
