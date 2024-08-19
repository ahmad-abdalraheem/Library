using Application.Repository;
using Application.Service;
using Domain.Entities;
using Infrastructure.FileModule;
using Moq;

namespace ConsoleApp.Tests;

public class BorrowScreenTest
{
	private readonly Mock<BookService> _mockBookService;
	private readonly Mock<MemberService> _mockMemberService;
	private readonly Mock<LibraryService> _mockLibraryService;
	private List<Book>? _booksList = new List<Book>();
	private List<Member>? _membersList = new List<Member>();
	private readonly BorrowScreen _borrowScreen;
	private readonly TestConsole _testConsole = new TestConsole();
	
	public BorrowScreenTest()
	{
		_mockBookService = new Mock<BookService>(new BookRepository(new BookHandler()));
		_mockMemberService = new Mock<MemberService>(new MemberRepository(new MemberHandler()));
		_mockLibraryService = new Mock<LibraryService>(_mockBookService.Object, _mockMemberService.Object);
		_borrowScreen = new BorrowScreen(_mockLibraryService.Object, _mockMemberService.Object, _testConsole);
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
	public void BorrowBookMenu_ShouldDisplayErrorMessage_IfBorrowedBookIsNull()
	{
		// Arrange
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(() => null);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Enter]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BorrowBookMenu_ShouldDisplayExitOption_IfBorrowedBookIsEmpty()
	{
		// Arrange
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(new List<Book>());
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.Enter]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.True(SearchInOutput("No Borrowed Books found"));
		Assert.True(SearchInOutput("Borrow new book"));
		Assert.True(SearchInOutput("Back to main menu"));
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BorrowBookMenu_ShouldDisplayBorrowBookOption_IfBorrowedBookIsEmpty()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = false},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = false},
		];
		_membersList = [
			new Member() {Id = 1, Name = "Member 1", Email = "member1@email.com"},
			new Member() {Id = 1, Name = "Member 2", Email = "member2@email.com"}
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		_mockLibraryService.Setup(ls => ls.BorrowBook(It.IsAny<Book>(), It.IsAny<Member>()))
			.Callback((Book book, Member member) =>
			{
				int index = _booksList.FindIndex(b => b.Id == book.Id);
				_booksList[index].IsBorrowed = true;
				_booksList[index].BorrowedDate = DateTime.Now;
				_booksList[index].BorrowedBy = member.Id;
			})
			.Returns(true);
		_mockMemberService.Setup(ms => ms.Get()).Returns(_membersList);
		
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Enter, ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Enter,
										ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Enter, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.True(_booksList[0].IsBorrowed);
		Assert.Equal(0, result);
	}

	[Fact]
	public void BorrowBookMenu_ShouldDisplayBorrowedBooks_IfBorrowedBookIsNotEmpty()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = true},
		];
		_membersList = [
			new Member() {Id = 1, Name = "Member 1", Email = "member1@email.com"},
			new Member() {Id = 1, Name = "Member 2", Email = "member2@email.com"}
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BorrowOperation_ShouldReturnBook_IfEnterKeyIsPressed()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = true},
		];
		_membersList = [
			new Member() {Id = 1, Name = "Member 1", Email = "member1@email.com"},
			new Member() {Id = 1, Name = "Member 2", Email = "member2@email.com"}
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		_mockLibraryService.Setup(ls => ls.ReturnBook(It.IsAny<int>()))
			.Callback((int id) =>
			{
				int index = _booksList.FindIndex(b => b.Id == id);
				_booksList[index].IsBorrowed = false;
				_booksList[index].BorrowedDate = null;
				_booksList[index].BorrowedBy = null;
			})
			.Returns(true);
		_mockMemberService.Setup(ms => ms.Get()).Returns(_membersList);
		
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Enter, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void BorrowOperation_ShouldBorrowBook_IfAddKeyIsPressed()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = true},
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(() => null);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Backspace, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}

	[Fact]
	public void BorrowBook_ShouldReturn_IfMembersIsNull()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = true},
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		_mockMemberService.Setup(ms => ms.Get()).Returns(() => null);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Backspace, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void SelectAvailableBook_ShouldReturnNull_IdBackspaceIsPressed()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = false},
		];
		_membersList = [
			new Member() {Id = 1, Name = "Member 1", Email = "member1@email.com"},
			new Member() {Id = 1, Name = "Member 2", Email = "member2@email.com"}
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		_mockMemberService.Setup(ms => ms.Get()).Returns(_membersList);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Backspace, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void SelectAvailableMember_ShouldReturnNull_IdBackspaceIsPressed()
	{
		// Arrange
		_booksList = [
			new Book("Book 1", "Author 1") {Id = 1, Title = "Book 1", Author = "Author 1" ,IsBorrowed = true},
			new Book("Book 2", "Author 1") {Id = 2, Title = "Book 2", Author = "Author 2" ,IsBorrowed = false},
		];
		_membersList = [
			new Member() {Id = 1, Name = "Member 1", Email = "member1@email.com"},
			new Member() {Id = 1, Name = "Member 2", Email = "member2@email.com"}
		];
		_mockLibraryService.Setup(ls => ls.GetBorrowed()).Returns(_booksList?.FindAll(b => b.IsBorrowed));
		_mockLibraryService.Setup(ls => ls.GetAvailable()).Returns(_booksList?.FindAll(b => b.IsBorrowed == false));
		_mockMemberService.Setup(ms => ms.Get()).Returns(_membersList);
		
		// Act
		_testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.Backspace]);
		int result = _borrowScreen.BorrowBookMenu();
		
		// Assert
		Assert.Equal(0, result);
	}
	
	[Fact]
	public void SearchInOutput_ShouldReturnFalse_WhenKeywordNotExist()
	{
		Assert.False(SearchInOutput("Ahmad Abdelkareem Nairat"));
	}
	
}