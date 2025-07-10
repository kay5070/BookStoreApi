using AutoMapper;
using BookStoreApi.Dtos;
using BookStoreApi.Models;

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