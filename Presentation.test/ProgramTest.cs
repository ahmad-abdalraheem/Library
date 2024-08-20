using Application.Repository;
using Application.Service;
using AutoMockFixture.Moq4;
using ConsoleApp;
using Domain.Entities;
using Domain.Repository;
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

	public ProgramTests()
	{
		string memberFilePath = "/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Members.json";
		string bookFilePath = "/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Books.json";

		_memberService = new MemberService(new MemberRepository(new FileHandler<Member>(memberFilePath)));
		_bookService = new BookService(new BookRepository(new FileHandler<Book>(bookFilePath)));

		_mockLibraryService = new Mock<LibraryService>(_bookService, _memberService);
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

		var serviceProvider = host.Services;

		Assert.NotNull(serviceProvider.GetService<MemberService>());
		Assert.NotNull(serviceProvider.GetService<BookService>());
		Assert.NotNull(serviceProvider.GetService<LibraryService>());
	}

	[Fact]
	public void RunApplication_ExecutesSuccessfully()
	{
		// Arrange
		var mockHost = new Mock<IHost>();
		var mockServiceProvider = new Mock<IServiceProvider>();

		var mockMemberService = new Mock<MemberService>(new Mock<IMemberRepository>().Object);
		var mockBookService = new Mock<BookService>(new Mock<IBookRepository>().Object);
		var mockLibraryService = new Mock<LibraryService>(mockBookService.Object, mockMemberService.Object);
		var console = new TestConsole();
		console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);

		mockServiceProvider.Setup(sp => sp.GetService(typeof(MemberService))).Returns(mockMemberService.Object);
		mockServiceProvider.Setup(sp => sp.GetService(typeof(BookService))).Returns(mockBookService.Object);

		mockHost.Setup(h => h.Services).Returns(mockServiceProvider.Object);

		var result = Program.RunApplication(mockHost.Object, console);

		Assert.Equal(0, result);
	}


	[Fact]
	public void RunMenu_SelectMembers_CallsMembersMenu()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([
			ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow,
			ConsoleKey.Enter
		]);

		int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);

		Assert.Equal(0, exitCode);
	}

	[Fact]
	public void RunMenu_SelectBooks_CallsBooksMenu()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([
			ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow, ConsoleKey.DownArrow,
			ConsoleKey.DownArrow, ConsoleKey.Enter
		]);

		int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);

		Assert.Equal(0, exitCode);
	}

	[Fact]
	public void RunMenu_SelectBorrowReturn_CallsBorrowMenu()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([
			ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Backspace, ConsoleKey.DownArrow,
			ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter
		]);

		int exitCode = Program.RunMenu(_memberService, _bookService, _mockLibraryService.Object, console);

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