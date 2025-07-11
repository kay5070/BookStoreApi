using Xunit;
using BookStoreApi.Services;
using BookStoreApi.Data;
using BookStoreApi.Models;
using BookStoreApi.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookStoreApi.Profiles;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MockQueryable.Moq;
using Moq;
using Shouldly;

namespace BookStoreApi.Tests.Services;

public class BooksServiceTests
{
    private readonly BooksService _service;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BooksServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BooksTestDb")
            .Options;

        _context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });

        _mapper = config.CreateMapper();

        _service = new BooksService(_context, _mapper);
    }

    [Fact]
    public void GetAll_ReturnsListOfBooks()
    {
        // Arrange
        _context.Books.Add(new Book
        {
            Title = "Test Book",
            Author = "Author",
            Year = 2023,
            Price = 199
        });
        _context.SaveChanges();

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.Single(result); // should return one book
        Assert.Equal("Test Book", result.First().Title);
    }

    [Fact]
    public void GetBookById_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_GetByIdTest")
            .Options;
        using var context = new AppDbContext(options);

        var book = new Book
        {
            Id = 1,
            Title = "Sample Book",
            Author = "John Doe",
            Year = 2020,
            Price = 99.99m
        };
        context.Books.Add(book);
        context.SaveChanges();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });

        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);
        
        // Act
        var result = service.GetById(1);
        
        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Sample Book");
        result.Author.Should().Be("John Doe");
        result.Year.Should().Be(2020);
        result.Price.Should().Be(99.99m);

    }

    [Fact]
    public void GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
    {
        // 1. Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_GetById_NullTest")
            .Options;

        using var context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });

        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);
        
        // 2. Act
        var result = service.GetById(999);
        // 3. Assert
        result.Should().BeNull();

    }

    [Fact]
    public void Create_ShouldAddBookToDatabase_AndReturnBookReadDto()
    {
        // 1. Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("bookDb_CreateTest")
            .Options;

        using var context = new AppDbContext(options);
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });
        var mapper = config.CreateMapper();

        var service = new BooksService(context, mapper);

        var newBookDto = new BookCreateDto
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Year = 2008,
            Price = 150
        };
        
        // 2. Act
        var result = service.Create(newBookDto);
        
        // 3. Assert
        var bookInDb = context.Books.FirstOrDefault();

        result.Should().NotBeNull();
        result.Title.Should().Be(newBookDto.Title);
        result.Author.Should().Be(newBookDto.Author);
        result.Year.Should().Be(newBookDto.Year);
        result.Price.Should().Be(newBookDto.Price);

        bookInDb.Should().NotBeNull();
        bookInDb!.Title.Should().Be(newBookDto.Title);
    }

    [Fact]
    public void Update_ShouldReturnTrueAndModifyBook_WhenBookExists()
    {
        // 1. Arrange - Isolated context and mapper
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"BookDb_Update_{Guid.NewGuid()}")
            .Options;

        using var context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });

        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        var book = new Book
        {
            Id = 1,
            Title = "Original Title",
            Author = "Original Author",
            Year = 2000,
            Price = 100
        };

        context.Books.Add(book);
        context.SaveChanges();

        var updateDto = new BookUpdateDto
        {
            Title = "Updated Title",
            Author = "Updated Author",
            Year = 2024,
            Price = 150
        };

        // 2. Act
        var result = service.Update(1, updateDto);

        // 3. Assert
        result.ShouldBeTrue();

        var updatedBook = context.Books.Find(1);

        updatedBook.Title.ShouldBe("Updated Title");
        updatedBook.Author.ShouldBe("Updated Author");
        updatedBook.Year.ShouldBe(2024);
        updatedBook.Price.ShouldBe(150);
    }

    [Fact]
    public void UpdateBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Update_Fail")
            .Options;

        using var context = new AppDbContext(options);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        var updateDto = new BookUpdateDto
        {
            Title = "New Title",
            Author = "New Author",
            Year = 2023,
            Price = 99.99m
        };

        // Act
        var result = service.Update(99, updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void PatchBook_ShouldReturnTrue_WhenPatchIsValid()
    {
        // 1. Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Patch_Success")
            .Options;

        using var context = new AppDbContext(options);
        context.Books.Add(new Book
        {
            Id = 1,
            Title = "Original Title",
            Author = "Original Author",
            Year = 2000,
            Price = 50
        });
        context.SaveChanges();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, "Patched Title");

        var modelState = new ModelStateDictionary();
        // 2. Act
        var result = service.PatchBook(1, patchDoc, modelState);
        
        // 3. Assert
        result.Should().BeTrue();
        context.Books.Find(1)!.Title.Should().Be("Patched Title");

    }
    [Fact]
    public void PatchBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Patch_NotFound")
            .Options;

        using var context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, "New Title");

        var modelState = new ModelStateDictionary();

        // Act
        var result = service.PatchBook(999, patchDoc, modelState);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void PatchBook_ShouldReturnFalse_WhenModelStateIsInvalid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Patch_InvalidModel")
            .Options;

        using var context = new AppDbContext(options);

        var book = new Book
        {
            Id = 1,
            Title = "Original Title",
            Author = "Original Author",
            Year = 2000,
            Price = 50
        };
        context.Books.Add(book);
        context.SaveChanges();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();

        var service = new BooksService(context, mapper);

        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, new string('a', 101)); // too long title

        var modelState = new ModelStateDictionary();

        // ❗️Manually simulate invalid state (since TryValidateModel isn't available here)
        modelState.AddModelError("Title", "Title must be at most 100 characters");

        // Act
        var result = service.PatchBook(1, patchDoc, modelState);

        // Assert
        result.Should().BeFalse();
    }


    [Fact]
    public void Delete_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Delete_Success")
            .Options;

        using var context = new AppDbContext(options);
        context.Books.Add(new Book
        {
            Id = 1,
            Title = "Delete Me",
            Author = "Author",
            Year = 2010,
            Price = 80
        });
        context.SaveChanges();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        // Act
        var result = service.Delete(1);

        // Assert
        result.Should().BeTrue();
        context.Books.Find(1).Should().BeNull(); // book should be deleted
    }
    [Fact]
    public void Delete_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDb_Delete_NotFound")
            .Options;

        using var context = new AppDbContext(options);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<BooksProfile>());
        var mapper = config.CreateMapper();
        var service = new BooksService(context, mapper);

        // Act
        var result = service.Delete(999); // non-existent ID

        // Assert
        result.Should().BeFalse();
    }

}
