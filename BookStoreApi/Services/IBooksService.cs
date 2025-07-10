using BookStoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Services;

public interface IBooksService
{
    IEnumerable<BookReadDto> GetAll();
    BookReadDto? GetById(int id);
    BookReadDto Create(BookCreateDto bookDto);
    bool Update(int id, BookUpdateDto bookDto);
    bool PatchBook(int id, JsonPatchDocument<BookPatchDto> patchDoc, ModelStateDictionary modelState);
    bool Delete(int id);
}