using BookStoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public interface IBooksService
{
    Task<IEnumerable<BookReadDto>> GetAllAsync();
    Task<BookReadDto?> GetByIdAsync(int id);
    Task<BookReadDto> CreateAsync(BookCreateDto bookDto);
    Task<bool> UpdateAsync(int id, BookUpdateDto bookDto);
    Task<bool> PatchBookAsync(int id, JsonPatchDocument<BookPatchDto> patchDoc, ModelStateDictionary modelState);
    Task<bool> DeleteAsync(int id);
}