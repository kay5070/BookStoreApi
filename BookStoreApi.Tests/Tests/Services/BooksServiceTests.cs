using Xunit;
using BookStoreApi.Services;
using BookStoreApi.Repositories;
using BookStoreApi.Models;
using BookStoreApi.Dtos;
using BookStoreApi.Profiles;
using FluentAssertions;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStoreApi.Tests.Services;

public class BooksServiceTests
{
    private readonly Mock<IBookRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly BooksService _service;

    public BooksServiceTests()
    {
        _mockRepo = new Mock<IBookRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BooksProfile>();
        });

        _mapper = config.CreateMapper();
        _service = new BooksService(_mockRepo.Object, _mapper);
    }

    [Fact]
    public void GetAll_ShouldReturnAllBooks()
    {
        // Arrange
        var books = new List<Book> { new Book { Id = 1, Title = "Book 1", Author = "Author", Year = 2023, Price = 10 } };
        _mockRepo.Setup(r => r.GetAll()).Returns(books);

        // Act
        var result = _service.GetAll();

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Book 1");
    }

    [Fact]
    public void GetById_ShouldReturnBook_WhenFound()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Book", Author = "Author", Year = 2022, Price = 100 };
        _mockRepo.Setup(r => r.GetById(1)).Returns(book);

        // Act
        var result = _service.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Book");
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(1)).Returns((Book?)null);

        // Act
        var result = _service.GetById(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldAddBook_AndReturnDto()
    {
        // Arrange
        var bookDto = new BookCreateDto
        {
            Title = "New Book",
            Author = "Author",
            Year = 2024,
            Price = 99
        };

        _mockRepo.Setup(r => r.Add(It.IsAny<Book>()));
        _mockRepo.Setup(r => r.Save());

        // Act
        var result = _service.Create(bookDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(bookDto.Title);
        result.Author.Should().Be(bookDto.Author);
        _mockRepo.Verify(r => r.Add(It.IsAny<Book>()), Times.Once);
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public void Update_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Old", Author = "Old", Year = 2000, Price = 50 };
        var updateDto = new BookUpdateDto { Title = "New", Author = "New", Year = 2023, Price = 120 };

        _mockRepo.Setup(r => r.GetById(1)).Returns(book);
        _mockRepo.Setup(r => r.Save());

        // Act
        var result = _service.Update(1, updateDto);

        // Assert
        result.Should().BeTrue();
        book.Title.Should().Be("New");
        book.Author.Should().Be("New");
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public void Update_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        var updateDto = new BookUpdateDto { Title = "X", Author = "Y", Year = 2023, Price = 100 };
        _mockRepo.Setup(r => r.GetById(1)).Returns((Book?)null);

        // Act
        var result = _service.Update(1, updateDto);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Save(), Times.Never);
    }

    [Fact]
    public void Delete_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Delete Me", Author = "Author", Year = 2010, Price = 80 };
        _mockRepo.Setup(r => r.GetById(1)).Returns(book);
        _mockRepo.Setup(r => r.Delete(book));
        _mockRepo.Setup(r => r.Save());

        // Act
        var result = _service.Delete(1);

        // Assert
        result.Should().BeTrue();
        _mockRepo.Verify(r => r.Delete(book), Times.Once);
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public void Delete_ShouldReturnFalse_WhenBookNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(1)).Returns((Book?)null);

        // Act
        var result = _service.Delete(1);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Delete(It.IsAny<Book>()), Times.Never);
        _mockRepo.Verify(r => r.Save(), Times.Never);
    }

    [Fact]
    public void PatchBook_ShouldReturnTrue_WhenValidPatch()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Old", Author = "Author", Year = 2000, Price = 50 };
        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, "Updated");

        var modelState = new ModelStateDictionary();

        _mockRepo.Setup(r => r.GetById(1)).Returns(book);
        _mockRepo.Setup(r => r.Save());

        // Act
        var result = _service.PatchBook(1, patchDoc, modelState);

        // Assert
        result.Should().BeTrue();
        book.Title.Should().Be("Updated");
        _mockRepo.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public void PatchBook_ShouldReturnFalse_WhenBookNotFound()
    {
        // Arrange
        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, "Title");

        var modelState = new ModelStateDictionary();
        _mockRepo.Setup(r => r.GetById(1)).Returns((Book?)null);

        // Act
        var result = _service.PatchBook(1, patchDoc, modelState);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void PatchBook_ShouldReturnFalse_WhenModelStateInvalid()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Old Title", Author = "Author", Year = 2000, Price = 50 };
        var patchDoc = new JsonPatchDocument<BookPatchDto>();
        patchDoc.Replace(b => b.Title, new string('A', 101)); // too long

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Title", "Title is too long");

        _mockRepo.Setup(r => r.GetById(1)).Returns(book);

        // Act
        var result = _service.PatchBook(1, patchDoc, modelState);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Save(), Times.Never);
    }
}
