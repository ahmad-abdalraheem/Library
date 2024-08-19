using Application.Repository;
using Application.Service;
using Domain.Entities;
using Infrastructure.FileModule;
using Moq;

namespace ConsoleApp.Tests;

public class BooksScreenTest
{
	private readonly Mock<BookService> _mockBookService;
	private readonly TestConsole _testConsole = new TestConsole();
	private readonly BooksScreen _booksScreen;
	private List<Book>? _booksList = new List<Book>();
	
	public BooksScreenTest()
	{
		_mockBookService = new Mock<BookService>(new BookRepository(new BookHandler()));
		_booksScreen = new BooksScreen(_mockBookService.Object, _testConsole);
	}
	
	private bool SearchInOutput(string searchString)
	{
		foreach (string line in _testConsole.Output)
		{
			if(line.Contains(searchString))
				return true;
		}

		return false;
	}

	[Fact]
	public void BooksMenu_ShouldDisplayErrorMessage_WhenBooksListIsNull()
	{
		// Arrange
		_mockBookService.Setup(bs => bs.Get()).Returns(() => null);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Enter]);
		int result = _booksScreen.BooksMenu();
		
		// Assert
		Assert.True(SearchInOutput("Error While loading Data"));
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BooksMenu_ShouldDisplayOptions_WhenBooksListIsEmpty()
	{
		// Arrange
		_mockBookService.Setup(bs => bs.Get()).Returns(() => new List<Book>());
		
		// Act
		_testConsole.Clear();
		_testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.Enter]);
		int result = _booksScreen.BooksMenu();
		
		// Assert
		Assert.True(SearchInOutput("No Books found."));
		Assert.True(SearchInOutput("Add a new book"));
		Assert.True(SearchInOutput("Back to main menu."));
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BooksMenu_ShouldAllowAddOption_WhenBooksListIsEmpty()
	{
		// Arrange
		_booksList = new List<Book>();
		_mockBookService.Setup(bs => bs.Get()).Returns(_booksList);
		_mockBookService.Setup(bs => bs.Add(It.IsAny<Book>())).Callback((Book book) => _booksList.Add(book)).Returns(true);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Enter, ConsoleKey.Backspace]);
		_testConsole.AddInputSequence(["Book 1", "Author 1"]);
		int result = _booksScreen.BooksMenu();
		
		// Assert
		Assert.Single(_booksList);
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BooksMenu_DisplayBooks_WhenBooksListIsNotEmpty()
	{
		// Arrange
		_booksList =
		[
			new("", "") { Id = 1, Title = "Book 1", Author = "Author 1" },
			new("", "") { Id = 2, Title = "Book 2", Author = "Author 2" }
		];
		_mockBookService.Setup(bs => bs.Get()).Returns(_booksList);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Backspace]);
		int result = _booksScreen.BooksMenu();
		
		// Assert
		Assert.True(SearchInOutput("Book 1"));
		Assert.True(SearchInOutput("Book 2"));
		Assert.True(SearchInOutput("Author 1"));
		Assert.True(SearchInOutput("Author 2"));
		Assert.Equal(0, result);
	}

	[Fact]
	public void BooksOperation_AddBook_WhenAddKeyIsPressed()
	{
		// Arrange
		_booksList =
		[
			new("", "") { Id = 1, Title = "Book 1", Author = "Author 1" },
			new("", "") { Id = 2, Title = "Book 2", Author = "Author 2" },
			new("", "") { Id = 3, Title = "Book 3", Author = "Author 3" }
		];
		_mockBookService.Setup(bs => bs.Get()).Returns(_booksList);
		_mockBookService.Setup(bs => bs.Add(It.IsAny<Book>())).Callback((Book book) => _booksList.Add(book)).Returns(true);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Backspace]);
		_testConsole.AddInputSequence(["", "Book 4", "", "Author 4"]);
		int result = _booksScreen.BooksMenu();
		
		Assert.Equal(4, _booksList.Count);
		Assert.Equal("Book 4", _booksList[3].Title);
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BooksOperation_UpdateSelectedBook_WhenEnterKeyIsPressed()
	{
		// Arrange
		_booksList =
		[
			new("", "") { Id = 1, Title = "Book 1", Author = "Author 1" },
			new("", "") { Id = 2, Title = "Book 2", Author = "Author 2" },
			new("", "") { Id = 3, Title = "Book 3", Author = "Author 3" }
		];
		_mockBookService.Setup(bs => bs.Get()).Returns(_booksList);
		_mockBookService.Setup(bs => bs.Update(It.IsAny<Book>())).Callback((Book book) =>
		{
			int index = _booksList.FindIndex(b => b.Id == book.Id);
			_booksList[index].Title = book.Title;
			_booksList[index].Author = book.Author;
		}).Returns(true);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Enter, ConsoleKey.Backspace]);
		_testConsole.AddInputSequence(["new Book 1", ""]);
		int result = _booksScreen.BooksMenu();
		
		Assert.Equal(3, _booksList.Count);
		Assert.Equal("new Book 1", _booksList[0].Title);
		Assert.Equal("Author 1", _booksList[0].Author);
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BooksOperation_DeleteSelectedBook_WhenDeleteKeyIsPressed()
	{
		// Arrange
		_booksList =
		[
			new("", "") { Id = 1, Title = "Book 1", Author = "Author 1" },
			new("", "") { Id = 2, Title = "Book 2", Author = "Author 2" },
			new("", "") { Id = 3, Title = "Book 3", Author = "Author 3" }
		];
		_mockBookService.Setup(bs => bs.Get()).Returns(_booksList);
		_mockBookService.Setup(bs => bs.Delete(It.IsAny<int>())).Callback((int id) => { _booksList.RemoveAll(b => b.Id == id); }).Returns(true);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.UpArrow, ConsoleKey.UpArrow, ConsoleKey.Delete, ConsoleKey.Backspace]);
		int result = _booksScreen.BooksMenu();
		
		Assert.Equal(2, _booksList.Count);
		Assert.Equal("Book 2", _booksList[0].Title);
		Assert.Equal("Author 2", _booksList[0].Author);
		Assert.Equal(0, result);
	}

	[Fact]
	public void SearchInOutput_ShouldReturnFalse_WhenKeywordNotExist()
	{
		Assert.False(SearchInOutput("Ahmad Abdelkareem Nairat"));
	}
}