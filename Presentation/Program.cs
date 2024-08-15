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

		return RunMenu(memberService, bookService, libraryService);
	}

	private static int RunMenu(MemberService memberService, BookService bookService, LibraryService libraryService)
	{
		MembersScreen? membersScreen = null;
		BooksScreen? booksScreen = null;
		BorrowScreen? borrowScreen = null;

		Console.WriteLine(Ansi.HideCursor);
		List<string> options = new List<string> { "Members", "Books", "Return/Borrow book", "Exit" };

		while (true)
		{
			Console.Clear();
			var selection = UserInteraction.GetUserSelection(options);
			switch (selection)
			{
				case 0:
					(membersScreen ??= new MembersScreen(memberService)).MembersMenu();
					break;
				case 1:
					(booksScreen ??= new BooksScreen(bookService)).BooksMenu();
					break;
				case 2:
					(borrowScreen ??= new BorrowScreen(libraryService, memberService)).BorrowBookMenu();
					break;
				default:
					return 0;
			}
		}
	}
}