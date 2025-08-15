using AutoMapper;
using BookStoreApi.Application.Dtos;
using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Profiles;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookReadDto>().ForMember(dest=>dest.Author,
            opt=>opt.MapFrom(s=>s.Author.FirstName+" " +s.Author.LastName));
        CreateMap<BookCreateDto, Book>();
        CreateMap<BookUpdateDto, Book>();
    }
}