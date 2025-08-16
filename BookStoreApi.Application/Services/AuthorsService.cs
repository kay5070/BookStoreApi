using AutoMapper;
using BookStoreApi.Application.Dtos;
using BookStoreApi.Application.Interfaces;
using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Application.Services;

public class AuthorsService:IAuthorsService
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public AuthorsService(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AuthorReadDto>> GetAllAsync()
    {
        var authors = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorReadDto>>(authors);
    }

    public async Task<AuthorReadDto?> GetByIdAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id);
        return _mapper.Map<AuthorReadDto>(author);
    }

    public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        await _repository.AddAsync(author);
        await _repository.SaveAsync();

        var retrievedAuthor = await _repository.GetByIdAsync(author.Id);
        return _mapper.Map<AuthorReadDto>(retrievedAuthor);
    }

    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto)
    {
        var retrievedAuthor = await _repository.GetByIdAsync(id);
        if (retrievedAuthor == null) return false;
        
         _mapper.Map(authorDto,retrievedAuthor);
        _repository.Update(retrievedAuthor);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var retrievedAuthor = await _repository.GetByIdAsync(id);
        if (retrievedAuthor == null) return false;
        
        _repository.Delete(retrievedAuthor);
        await _repository.SaveAsync();
        return true;
    }
}