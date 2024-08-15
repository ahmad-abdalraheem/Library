using Application.Service;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Application.Tests;

public class BookServiceTests
{
	private readonly BookService _bookService;
	private readonly Mock<IBookRepository> _mockBookRepository;

	public BookServiceTests()
	{
		_mockBookRepository = new Mock<IBookRepository>();
		_bookService = new BookService(_mockBookRepository.Object);
	}

	[Fact]
	public void Add_ShouldReturnTrue_WhenAddSucceeds()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Test Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Add(book)).Returns(true);

		var result = _bookService.Add(book);

		Assert.True(result);
	}

	[Fact]
	public void Add_ShouldReturnFalse_WhenAddFails()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Test Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Add(book)).Returns(false);

		var result = _bookService.Add(book);

		Assert.False(result);
	}

	[Fact]
	public void Add_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Test Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Add(book)).Throws(new Exception("Database error"));

		var result = _bookService.Add(book);

		Assert.False(result);
	}

	[Fact]
	public void Update_ShouldReturnTrue_WhenUpdateSucceeds()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Updated Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Update(book)).Returns(true);

		var result = _bookService.Update(book);

		Assert.True(result);
	}

	[Fact]
	public void Update_ShouldReturnFalse_WhenUpdateFails()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Updated Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Update(book)).Returns(false);

		var result = _bookService.Update(book);

		Assert.False(result);
	}

	[Fact]
	public void Update_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		var book = new Book
		{
			Id = 1,
			Title = "Updated Book",
			Author = "Some Author"
		};
		_mockBookRepository.Setup(repo => repo.Update(book)).Throws(new Exception("Database error"));

		var result = _bookService.Update(book);

		Assert.False(result);
	}

	[Fact]
	public void Delete_ShouldReturnTrue_WhenDeleteSucceeds()
	{
		_mockBookRepository.Setup(repo => repo.Delete(1)).Returns(true);

		var result = _bookService.Delete(1);

		Assert.True(result);
	}

	[Fact]
	public void Delete_ShouldReturnFalse_WhenDeleteFails()
	{
		_mockBookRepository.Setup(repo => repo.Delete(1)).Returns(false);

		var result = _bookService.Delete(1);

		Assert.False(result);
	}

	[Fact]
	public void Delete_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		_mockBookRepository.Setup(repo => repo.Delete(1)).Throws(new Exception("Database error"));

		var result = _bookService.Delete(1);

		Assert.False(result);
	}

	[Fact]
	public void Get_ShouldReturnBooks_WhenGetSucceeds()
	{
		var books = new List<Book>
		{
			new()
			{
				Id = 1,
				Title = "Book 1",
				Author = "Author 1"
			},
			new()
			{
				Id = 2,
				Title = "Book 2",
				Author = "Author 2"
			}
		};
		_mockBookRepository.Setup(repo => repo.Get()).Returns(books);

		var result = _bookService.Get();

		Assert.NotNull(result);
		Assert.Equal(2, result.Count);
	}

	[Fact]
	public void Get_ShouldReturnNull_WhenGetFails()
	{
		_mockBookRepository.Setup(repo => repo.Get()).Returns((List<Book>)null);

		var result = _bookService.Get();

		Assert.Null(result);
	}

	[Fact]
	public void Get_ShouldReturnNull_WhenExceptionIsThrown()
	{
		_mockBookRepository.Setup(repo => repo.Get()).Throws(new Exception("Database error"));

		var result = _bookService.Get();

		Assert.Null(result);
	}
}