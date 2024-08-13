using Application.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp;

public static class Program
{
	public static int Main()
	{
		var host = Host.CreateDefaultBuilder()
			.ConfigureServices(services =>
			{
				services.AddApplicationServices();
			})
			.Build();

		var memberService = host.Services.GetRequiredService<MemberService>();
		var bookService = host.Services.GetRequiredService<BookService>();
		var libraryService = new LibraryService(bookService, memberService);

		// Screens
		MembersScreen? membersScreen = null;
		BooksScreen? booksScreen = null;
		BorrowScreen? borrowScreen = null;

		Console.WriteLine(Ansi.HideCursor);
		List<string> options = ["Members", "Books", "Return/Borrow book", "Exit"];

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