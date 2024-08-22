using Application.Service;
using ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class Program
{
	public static int Main()
	{
		IConsole console = new UserConsole();
		try
		{
			console.ReadKey();
		}
		catch (Exception)
		{
			return 0;
		}

		IHost host = CreateHost();
		return RunApplication(host, console);
	}

	public static IHost CreateHost()
	{
		return Host.CreateDefaultBuilder()
			.ConfigureServices(services => services.AddApplicationServices())
			.Build();
	}

	public static int RunApplication(IHost host, IConsole console)
	{
		var memberService = host.Services.GetRequiredService<MemberService>();
		var bookService = host.Services.GetRequiredService<BookService>();
		var libraryService = new LibraryService(bookService, memberService);

		return RunMenu(memberService, bookService, libraryService, console);
	}

	public static int RunMenu(MemberService memberService, BookService bookService,
		LibraryService libraryService, IConsole console)
	{
		MembersScreen? membersScreen = null;
		BooksScreen? booksScreen = null;
		BorrowScreen? borrowScreen = null;

		console.WriteLine(Ansi.HideCursor);
		List<string> options = ["Members", "Books", "Return/Borrow book", "Exit"];

		while (true)
		{
			console.Clear();
			var selection = UserInteraction.GetUserSelection(options, console: console);
			switch (selection)
			{
				case 0:
					(membersScreen ??= new MembersScreen(memberService, console)).MembersMenu();
					break;
				case 1:
					(booksScreen ??= new BooksScreen(bookService, console)).BooksMenu();
					break;
				case 2:
					(borrowScreen ??= new BorrowScreen(libraryService, memberService, console)).BorrowBookMenu();
					break;
				default:
					return 0;
			}
		}
	}
}