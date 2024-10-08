using Application.Repository;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.DataHandler;
using Moq;

namespace Application.Tests;

public class BookRepositoryTests
{
	private readonly BookRepository _bookRepository;
	private readonly Mock<IDataHandler<Book>> _mockBookHandler;

	public BookRepositoryTests()
	{
		_mockBookHandler = new Mock<IDataHandler<Book>>();
		_bookRepository = new BookRepository(_mockBookHandler.Object);
	}

	[Fact]
	public void Add_ShouldAddBook()
	{
		var initialBooks = new List<Book>
		{
			new() { Id = 1, Title = "Existing Book", Author = "Existing Author" }
		};
		var newBook = new Book() { Id = 2, Title = "New Book", Author = "New Author" };
		_mockBookHandler.Setup(handler => handler.Get()).Returns(initialBooks);
		_mockBookHandler.Setup(handler => handler.Add(It.IsAny<Book>())).Returns(true);

		var result = _bookRepository.Add(newBook);

		Assert.True(result);
		Assert.Equal(2, initialBooks.Count);
		Assert.Equal("New Book", initialBooks[1].Title);
		_mockBookHandler.Verify(handler => handler.Add(It.IsAny<Book>()), Times.Once);
	}

	[Fact]
	public void Update_ShouldUpdateBook()
	{
		var initialBooks = new List<Book>
		{
			new() { Id = 1, Title = "Old Title", Author = "Old Author" }
		};
		var updatedBook = new Book() { Id = 1, Title = "Updated Title", Author = "Updated Author" };
		_mockBookHandler.Setup(handler => handler.Get()).Returns(initialBooks);
		_mockBookHandler.Setup(handler => handler.Update(It.IsAny<Book>())).Returns(true);

		var result = _bookRepository.Update(updatedBook);

		Assert.True(result);
		Assert.Equal("Updated Title", initialBooks[0].Title);
		_mockBookHandler.Verify(handler => handler.Update(It.IsAny<Book>()), Times.Once);
	}

	[Fact]
	public void Delete_ShouldRemoveBook()
	{
		var initialBooks = new List<Book>
		{
			new() { Id = 1, Title = "Book to Remove", Author = "Author" },
			new() { Id = 2, Title = "Another Book", Author = "Author" }
		};
		_mockBookHandler.Setup(handler => handler.Get()).Returns(initialBooks);
		_mockBookHandler.Setup(handler => handler.Delete(It.IsAny<int>())).Returns(true);

		var result = _bookRepository.Delete(1);

		Assert.True(result);
		Assert.Single(initialBooks);
		Assert.Equal(2, initialBooks[0].Id);
		_mockBookHandler.Verify(handler => handler.Delete(It.IsAny<int>()), Times.Once);
	}

	[Fact]
	public void Get_ShouldReturnBooks()
	{
		var books = new List<Book>
		{
			new() { Id = 1, Title = "Book 1", Author = "Author 1" },
			new() { Id = 2, Title = "Book 2", Author = "Author 2" }
		};
		_mockBookHandler.Setup(handler => handler.Get()).Returns(books);

		var result = _bookRepository.Get();

		Assert.NotNull(result);
		Assert.Equal(2, result.Count);
		Assert.Equal("Book 1", result[0].Title);
		Assert.Equal("Book 2", result[1].Title);
	}
	
	[Fact]
	public void GetById_ShouldReturnCorrectBook()
	{
		var books = new List<Book>
		{
			new() { Id = 1, Title = "Book 1", Author = "Author 1" },
			new() { Id = 2, Title = "Book 2", Author = "Author 2" }
		};
		_mockBookHandler.Setup(handler => handler.GetById(2)).Returns(books[1]);
	
		var result = _bookRepository.GetById(2);
	
		Assert.NotNull(result);
		Assert.Equal(2, result.Id);
		Assert.Equal("Book 2", result.Title);
	}

	[Fact]
	public void Add_ShouldThrowFileLoadExceptionWhenBooksAreNull()
	{
		_mockBookHandler.Setup(handler => handler.Get()).Returns((List<Book>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _bookRepository.Add(new Book()
		{
			Title = "undefined",
			Author = "undefined"
		}));
	}

	[Fact]
	public void Update_ShouldThrowFileLoadExceptionWhenBooksAreNull()
	{
		_mockBookHandler.Setup(handler => handler.Get()).Returns((List<Book>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _bookRepository.Update(new Book()
		{
			Title = "undefined",
			Author = "undefined"
		}));
	}

	[Fact]
	public void Delete_ShouldThrowFileLoadExceptionWhenBooksAreNull()
	{
		_mockBookHandler.Setup(handler => handler.Get()).Returns((List<Book>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _bookRepository.Delete(1));
	}
}