using AutoMapper;
using BookStoreApi.Application.Dtos;
using BookStoreApi.Domain.Entities;

namespace BookStoreApi.Profiles;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        //Book Mapping
        CreateMap<Book, BookReadDto>().ForMember(dest=>dest.Author,
            opt=>opt.MapFrom(s=>s.Author.FirstName+" " +s.Author.LastName));
        CreateMap<Book, BookBriefDto>();
        CreateMap<BookCreateDto, Book>();
        CreateMap<BookUpdateDto, Book>();
        
        // Author Mapping
        CreateMap<AuthorCreateDto, Author>();
        CreateMap<Author, AuthorReadDto>().ForMember(dest=>dest.Books,opt=>opt.MapFrom(src=>src.Books));
        CreateMap<AuthorUpdateDto, Author>();
    }
}