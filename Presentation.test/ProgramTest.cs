using Application.Repository;
using Application.Service;
using AutoMockFixture.Moq4;
using ConsoleApp;
using Infrastructure.FileModule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;
using Xunit.Abstractions;

public class ProgramTests
{
    private readonly MemberService _memberService;
    private readonly BookService _bookService;
    
    private readonly Mock<LibraryService> _mockLibraryService;
    private readonly Mock<MembersScreen> _mockMembersScreen;
    private readonly Mock<BooksScreen> _mockBooksScreen;
    private readonly Mock<BorrowScreen> _mockBorrowScreen;

    public ProgramTests()
    {
        _memberService = new MemberService(new MemberRepository(new MemberHandler()));
        _bookService = new BookService(new BookRepository(new BookHandler()));
        
        _mockLibraryService = new Mock<LibraryService>(_bookService, _memberService);
        
        _mockMembersScreen = new Mock<MembersScreen>(_memberService);
        _mockBooksScreen = new Mock<BooksScreen>(_bookService);
        _mockBorrowScreen = new Mock<BorrowScreen>(_mockLibraryService.Object, _memberService);
    }

    [Fact]
    public void RunMenu_SelectMembers_CallsMembersMenu()
    {
        TestConsole console = new TestConsole();
        console.AddKeySequence([ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
        
        int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);
        
        Assert.True(console.Output.Contains("ID\u001b[1;5HName\u001b[1;30HEmail")); // member page header
        Assert.Equal(0, exitCode);
    }
    
    [Fact]
    public void RunMenu_SelectBooks_CallsBooksMenu()
    {
        TestConsole console = new TestConsole();
        console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
        
        int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);
        
        Assert.True(console.Output.Contains("ID[1;5HTitle[1;40HAuthor[1;67HStatus[1;79H")); // Book page header
        Assert.Equal(0, exitCode);
    }
    
    [Fact]
    public void RunMenu_SelectBorrowReturn_CallsBorrowMenu()
    {
        TestConsole console = new TestConsole();
        console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
        
        int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);
        
        Assert.True(console.Output.Contains("ID[1;5HTitle[1;40HAuthor[1;68HBorrowed By[1;93HBorrowed Date")); // Borrow page header
        Assert.Equal(0, exitCode);
    }
    
    [Fact]
    public void RunMenu_SelectExit_EndProgram()
    {
        TestConsole console = new TestConsole();
        console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
        
        int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);
        
        Assert.Equal(0, exitCode);
    }
}
