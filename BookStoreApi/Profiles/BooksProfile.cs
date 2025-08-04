using AutoMapper;
using BookStoreApi.Domain.Entities;
using BookStoreApi.Dtos;

namespace BookStoreApi.Profiles;

public class BooksProfile:Profile
{
    public BooksProfile()
    {
        CreateMap<Book, BookReadDto>();
        CreateMap<BookCreateDto, Book>();
        CreateMap<BookUpdateDto, Book>();
        CreateMap<BookPatchDto, Book>().ReverseMap();
    }
}