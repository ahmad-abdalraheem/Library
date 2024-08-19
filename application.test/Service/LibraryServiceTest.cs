using System.Runtime.CompilerServices;
using Application.Service;
using Domain.Entities;
using Domain.Repository;
using Moq;
using Xunit;

public class LibraryServiceTests
{
    private readonly LibraryService _libraryService;
    private readonly BookService _bookService;
    private readonly MemberService _memberService;
    private List<Book> _bookList;
    private readonly List<Member> _memberList;

    public LibraryServiceTests()
    {
        _bookList = new List<Book>();
        _memberList = new List<Member>();

        var mockBookRepository = new Mock<IBookRepository>();
        var mockMemberRepository = new Mock<IMemberRepository>();

        mockBookRepository.Setup(repo => repo.Get()).Returns(() => _bookList);
        mockBookRepository.Setup(repo => repo.Add(It.IsAny<Book>())).Callback<Book>(book =>_bookList.Add(book)).Returns(true);
        mockBookRepository.Setup(repo => repo.Update(It.IsAny<Book>()))
            .Callback<Book>(book =>
            {
                var index = _bookList.FindIndex(b => b.Id == book.Id);
                if (index != -1) _bookList[index] = book;
            }).Returns(true);
        // mockBookRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
        //     .Callback<int>( id => { 
        //         int index = _bookList.FindIndex(b => b.Id == id);
        //         if (index != -1) _bookList.RemoveAt(index);
        //     }).Returns(true);

        mockMemberRepository.Setup(repo => repo.Get()).Returns(() => _memberList);
        mockMemberRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((int memberId) => _memberList.Find(m => m.Id == memberId));

        _bookService = new BookService(mockBookRepository.Object);
        _memberService = new MemberService(mockMemberRepository.Object);

        _libraryService = new LibraryService(_bookService, _memberService);
    }

    [Fact]
    public void BorrowBook_ShouldReturnTrue_WhenBookIsSuccessfullyBorrowed()
    {
        Book book = new Book ("", "") {Id = 1, Author = "John Doe", Title = "Book 1", IsBorrowed = false};
        _bookList.Add(book);
        
        Member member = new Member { Id = 2, Name = "undefined" };
        _memberList.Add(member);
        
        bool result = _libraryService.BorrowBook(book, member);

        Assert.True(result);
    }
    
    [Fact]
    public void BorrowBook_ShouldReturnFalse_WhenMemberIsNotExists()
    {
        Book book = new Book ("", "") {Id = 1, Author = "John Doe", Title = "Book 1", IsBorrowed = false};
        _bookList.Add(book);
        
        Member member = new Member { Id = 2, Name = "undefined" };
        _memberList.Clear();
        
        bool result = _libraryService.BorrowBook(book, member);

        Assert.False(result);
    }
    
    [Fact]
    public void BorrowBook_ShouldReturnFalse_WhenBookIsNull()
    {
        var member = new Member
        {
            Id = 2,
            Name = "undefined"
        };

        var result = _libraryService.BorrowBook(null, member);

        Assert.False(result);
    }
    

    [Fact]
    public void BorrowBook_ShouldReturnFalse_WhenUpdateFails()
    {
        var book = new Book ("", "")
        {
            Id = 1,
            IsBorrowed = false,
            Title = "Book 1",
            Author = "Author 1"
        };
        var member = new Member
        {
            Id = 2,
            Name = "undefined"
        };

        _bookList.Add(book);

        _bookService.Update(book);
        var result = _libraryService.BorrowBook(book, member);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAvailable_ShouldReturnAvailableBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book ("", "") { Id = 1, IsBorrowed = false, Title = "Book 1", Author = "Author 1" },
            new Book ("", "") { Id = 2, IsBorrowed = true, Title = "Book 2", Author = "Author 2" },
            new Book ("", "") { Id = 3, IsBorrowed = false, Title = "Book 3", Author = "Author 3" }
        };

        _bookList.AddRange(books);

        // Act
        var result = _libraryService.GetAvailable();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.False(b.IsBorrowed));
    }

    [Fact]
    public void GetBorrowed_ShouldReturnBorrowedBooks_WithMemberNames()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book("", "") { Id = 1, IsBorrowed = true, BorrowedBy = 2, Title = "Book 1", Author = "Author 1" },
            new Book("", "") { Id = 2, IsBorrowed = false, Title = "Book 2", Author = "Author 2" },
            new Book("", "") { Id = 3, IsBorrowed = true, BorrowedBy = 3, Title = "Book 3", Author = "Author 3" }
        };

        var members = new List<Member>
        {
            new Member { Id = 2, Name = "Member 2" },
            new Member { Id = 3, Name = "Member 3" }
        };

        _bookList.AddRange(books);
        _memberList.AddRange(members);

        // Act
        var result = _libraryService.GetBorrowed();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, b => b.MemberName == "Member 2");
        Assert.Contains(result, b => b.MemberName == "Member 3");
    }
    
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenBooksIsNullOrEmpty()
    {
        // Arrange
        _bookList.Clear();
        
        // Act
        var result = _libraryService.ReturnBook(1);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        _bookList = [ new Book("Book 1", "Author 1") {Id= 1, Title = "Book 1", Author = "Author 1"} ];
        
        // Act
        var result = _libraryService.ReturnBook(2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ReturnBook_ShouldReturnTrue_WhenBookIsSuccessfullyReturned()
    {
        // Arrange
        var book = new Book ("", "")
        {
            Id = 1,
            IsBorrowed = true,
            Title = "Book",
            Author = "Author"
        };
        _bookService.Add(book);

        // Act
        bool result = _libraryService.ReturnBook(1);

        // Assert
        Assert.True(result);
        Assert.False(book.IsBorrowed);
        Assert.Null(book.BorrowedBy);
        Assert.Null(book.BorrowedDate);
    }
}
