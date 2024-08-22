using Application.Repository;
using Application.Service;
using AutoMockFixture.Moq4;
using ConsoleApp;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.DataHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

public class ProgramTests
{
    private readonly Mock<MemberService> _mockMemberService;
    private readonly Mock<BookService> _mockBookService;
    private readonly Mock<LibraryService> _mockLibraryService;

    public ProgramTests()
    {
        _mockMemberService = new Mock<MemberService>(new Mock<IMemberRepository>().Object);
        _mockBookService = new Mock<BookService>(new Mock<IBookRepository>().Object);
        _mockLibraryService = new Mock<LibraryService>(_mockBookService.Object, _mockMemberService.Object);
    }

    [Fact]
    public void Main_IsRunSuccessfully()
    {
        int result = Program.Main();

        Assert.Equal(0, result);
    }

    [Fact]
    public void CreateHost_CreatesHostWithConfiguredServices()
    {
        var host = Program.CreateHost();

        Assert.NotNull(host);

        using (var scope = host.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            Assert.NotNull(serviceProvider.GetService<MemberService>());
            Assert.NotNull(serviceProvider.GetService<BookService>());
            Assert.NotNull(serviceProvider.GetService<LibraryService>());
        }
    }


    [Fact]
    public void RunApplication_ExecutesSuccessfully()
    {
        // Arrange
        var mockHost = new Mock<IHost>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        TestConsole console = new TestConsole();
        console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Backspace]);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(MemberService))).Returns(_mockMemberService.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(BookService))).Returns(_mockBookService.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(LibraryService))).Returns(_mockLibraryService.Object);

        mockHost.Setup(h => h.Services).Returns(mockServiceProvider.Object);

        var result = Program.RunApplication(mockHost.Object, console);

        Assert.Equal(0, result);
    }

    [Fact]
    public void RunMenu_SelectMembers_CallsMembersMenu()
    {
        // Arrange
        var console = new TestConsole();
        console.AddKeySequence([ ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter ]);

        // Act
        int exitCode = Program.RunMenu(_mockMemberService.Object, _mockBookService.Object, _mockLibraryService.Object, console);

        // Assert
        Assert.Equal(0, exitCode);
    }

    [Fact]
    public void RunMenu_SelectBooks_CallsBooksMenu()
    {
        // Arrange
        var console = new TestConsole();
        console.AddKeySequence([ ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter ]);

        // Act
        int exitCode = Program.RunMenu(_mockMemberService.Object, _mockBookService.Object, _mockLibraryService.Object, console);

        // Assert
        Assert.Equal(0, exitCode);
    }

    [Fact]
    public void RunMenu_SelectBorrowReturn_CallsBorrowMenu()
    {
        // Arrange
        var console = new TestConsole();
        console.AddKeySequence([ ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter ]);

        // Act
        int exitCode = Program.RunMenu(_mockMemberService.Object, _mockBookService.Object, _mockLibraryService.Object, console);

        // Assert
        Assert.Equal(0, exitCode);
    }

    [Fact]
    public void RunMenu_SelectExit_EndProgram()
    {
        // Arrange
        var console = new TestConsole();
        console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter ]);

        // Act
        int exitCode = Program.RunMenu(_mockMemberService.Object, _mockBookService.Object, _mockLibraryService.Object, console);

        // Assert
        Assert.Equal(0, exitCode);
    }
}
