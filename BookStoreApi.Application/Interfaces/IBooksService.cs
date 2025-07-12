using BookStoreApi.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Application.Interfaces;

public interface IBooksService
{
    IEnumerable<BookReadDto> GetAll();
    BookReadDto? GetById(int id);
    BookReadDto Create(BookCreateDto bookDto);
    bool Update(int id, BookUpdateDto bookDto);
    BookPatchDto? GetByIdForPatch(int id);
    bool ApplyPatch(int id, BookPatchDto patchedDto);

    bool Delete(int id);
}