using Application.Service;
using ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class Program
{
	public static int Main()
	{
		var host = Host.CreateDefaultBuilder()
			.ConfigureServices(services => services.AddApplicationServices())
			.Build();

		var memberService = host.Services.GetRequiredService<MemberService>();
		var bookService = host.Services.GetRequiredService<BookService>();
		var libraryService = new LibraryService(bookService, memberService);

		IConsole console = new UserConsole();
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