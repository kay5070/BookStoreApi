using AutoMapper;
using BookStoreApi.Domain.Entities;
using BookStoreApi.Dtos;
using BookStoreApi.Profiles;
using FluentAssertions;
using Moq;

namespace BookStoreApi.Tests.Tests.Services;

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
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var books = new List<Book> { new Book { Id = 1, Title = "Book 1", Author = "Author", Year = 2023, Price = 10 } };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

        // Act
        var result =await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Book 1");
    }

     [Fact]
     public async Task GetByIdAsync_ShouldReturnBook_WhenFound()
     {
         // Arrange
         var book = new Book { Id = 1, Title = "Book", Author = "Author", Year = 2022, Price = 100 };
         _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

         // Act
         var result = await _service.GetByIdAsync(1);

         // Assert
         result.Should().NotBeNull();
         result!.Title.Should().Be("Book");
     }

     [Fact]
     public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
     {
         // Arrange
         _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Book?)null);

         // Act
         var result =await _service.GetByIdAsync(1);

         // Assert
         result.Should().BeNull();
     }

     [Fact]
     public async Task CreateAsync_ShouldAddBook_AndReturnDto()
     {
         // Arrange
         var bookDto = new BookCreateDto
         {
             Title = "New Book",
             Author = "Author",
             Year = 2024,
             Price = 99
         };

         _mockRepo.Setup(r => r.AddAsync(It.IsAny<Book>()));
         _mockRepo.Setup(r => r.SaveAsync());

         // Act
         var result =await _service.CreateAsync(bookDto);

         // Assert
         result.Should().NotBeNull();
         result.Title.Should().Be(bookDto.Title);
         result.Author.Should().Be(bookDto.Author);
         _mockRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
         _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
     }

     [Fact]
     public async Task UpdateAsync_ShouldReturnTrue_WhenBookExists()
     {
         // Arrange
         var book = new Book { Id = 1, Title = "Old", Author = "Old", Year = 2000, Price = 50 };
         var updateDto = new BookUpdateDto { Title = "New", Author = "New", Year = 2023, Price = 120 };

         _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
         _mockRepo.Setup(r => r.SaveAsync());

         // Act
         var result =await _service.UpdateAsync(1, updateDto);

         // Assert
         result.Should().BeTrue();
         book.Title.Should().Be("New");
         book.Author.Should().Be("New");
         _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
     }

     [Fact]
     public async Task UpdateAsync_ShouldReturnFalse_WhenBookDoesNotExist()
     {
         // Arrange
         var updateDto = new BookUpdateDto { Title = "X", Author = "Y", Year = 2023, Price = 100 };
         _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Book?)null);

         // Act
         var result =await _service.UpdateAsync(1, updateDto);

         // Assert
         result.Should().BeFalse();
         _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
     }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Delete Me", Author = "Author", Year = 2010, Price = 80 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
        _mockRepo.Setup(r => r.Delete(book));
        _mockRepo.Setup(r => r.SaveAsync());

        // Act
        var result =await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepo.Verify(r => r.Delete(book), Times.Once);
        _mockRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBookNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Book?)null);

        // Act
        var result =await _service.DeleteAsync(1);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(r => r.Delete(It.IsAny<Book>()), Times.Never);
        _mockRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

}

