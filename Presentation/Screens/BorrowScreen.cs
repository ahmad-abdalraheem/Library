using Application.Service;
using Domain.Entities;
using static ConsoleApp.Ansi;

namespace ConsoleApp;

public class BorrowScreen(LibraryService libraryService, MemberService memberService, IConsole console)
{
	private List<Book>? BorrowedBooks { get; set; }
	private List<Book?>? AvailableBooks { get; set; }
	private List<Member>? Members { get; set; }

	public int BorrowBookMenu()
	{
		console.Clear();
		if ((BorrowedBooks ??= libraryService.GetBorrowed()) == null)
		{
			console.WriteLine(Red + "Error While loading Data." + Reset);
			console.ReadKey();
			return 0;
		}

		var isExit = false;
		while (!isExit)
			if (BorrowedBooks?.Count == 0)
			{
				console.Clear();
				console.WriteLine(Red + "No Borrowed Books found." + Reset);
				switch (UserInteraction.GetUserSelection(["Borrow new book.", "Back to main menu."], console))
				{
					case 0:
						BorrowBook();
						BorrowedBooks = libraryService.GetBorrowed();
						break;
					default:
						isExit = true;
						break;
				}
			}
			else
			{
				DisplayBorrowedBooks();
				isExit = BorrowOperation();
			}

		return 0;
	}

	private bool BorrowOperation()
	{
		var selection = 0;
		PrintRow(BorrowedBooks?[selection], 3, Blue);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						PrintRow(BorrowedBooks?[selection], selection + 3, Reset);
						selection--;
						PrintRow(BorrowedBooks?[selection], selection + 3, Blue);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selection < BorrowedBooks?.Count - 1)
					{
						PrintRow(BorrowedBooks?[selection], selection + 3, Reset);
						selection++;
						PrintRow(BorrowedBooks?[selection], selection + 3, Blue);
					}

					break;
				case ConsoleKey.Enter:
					if (libraryService.ReturnBook(BorrowedBooks![selection].Id))
						BorrowedBooks.RemoveAt(selection);
					return false;
				case ConsoleKey.Add:
					BorrowBook();
					BorrowedBooks = libraryService.GetBorrowed();
					return false;
				case ConsoleKey.Backspace:
					return true;
			}
	}

	private void BorrowBook()
	{
		console.Clear();
		if ((AvailableBooks ??= libraryService.GetAvailable()!) == null)
		{
			console.WriteLine(Red + "Error while loading data" + Reset);
			console.ReadKey();
			return;
		}

		if ((Members ??= memberService.Get()) == null)
		{
			console.WriteLine(Red + "Error while loading data" + Reset);
			console.ReadKey();
			return;
		}

		Book? book = SelectAvailableBook();
		if (book == null)
			return;
		Member? member = SelectMember();
		if (member == null)
			return;
//		libraryService.BorrowBook(book, member);
		AvailableBooks.Remove(book);
		BorrowedBooks?.Add(book);
	}

	private void DisplayBorrowedBooks()
	{
		console.Clear();

		var currentRow = 1;
		console.Write($"ID{CursorPosition(1, 5)}Title{CursorPosition(1, 40)}Author" +
		              $"{CursorPosition(1, 68)}Borrowed By" +
		              $"{CursorPosition(1, 93)}Borrowed Date");
		console.Write("\n______________________________________________________________\n" + Reset);
		currentRow += 2;
		if (BorrowedBooks != null)
			foreach (var book in BorrowedBooks)
				PrintRow(book, currentRow++, Reset);
		console.WriteLine(Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		console.WriteLine("- Enter Key -> Return the book");
		console.WriteLine("- Plus (+) Key -> Borrow a new book");
		console.WriteLine("- Backspace Key -> Get back to Main Menu." + Reset);
	}

	private Book? SelectAvailableBook()
	{
		console.Write(Yellow + "ID   Title                              Author\n" +
		              "__________________________________________________________\n" + Reset);
		var selection = 0;
		foreach (var book in AvailableBooks!)
		{
			console.Write(CursorPosition(selection + 3, 1) + book!.Id + CursorPosition(selection + 3, 5) +
			              (book.Title.Length > 30 ? book.Title.Substring(0, 30) : book.Title) +
			              CursorPosition(selection + 3, 40) + book.Author);
			selection++;
		}

		selection = 0;
		console.WriteLine("\nSelect a book to borrow, Backspace to get back");
		console.Write(Blue + CursorPosition(3, 1) + AvailableBooks[selection]!.Id + CursorPosition(3, 5) +
		              (AvailableBooks[selection]?.Title.Length > 30
			              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
			              : AvailableBooks[selection]?.Title)
		              + CursorPosition(3, 40) + AvailableBooks[selection]?.Author + Reset);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						console.Write(ToLineStart + AvailableBooks?[selection]?.Id + CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author);
						--selection;
						console.Write(CursorPosition(selection + 3, 1) + Blue + AvailableBooks?[selection]?.Id +
						              CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author + Reset);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selection < AvailableBooks?.Count - 1)
					{
						console.Write(ToLineStart + AvailableBooks?[selection]?.Id + CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author);
						++selection;
						console.Write(CursorPosition(selection + 3, 1) + Blue + AvailableBooks?[selection]?.Id +
						              CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author + Reset);
					}

					break;
				case ConsoleKey.Enter:
					return AvailableBooks?[selection];
				case ConsoleKey.Backspace:
					return null;
			}
	}

	private Member? SelectMember()
	{
		console.Clear();
		console.Write(Yellow + "ID     Name\n" + "______________________________\n" + Reset);
		var selection = 0;
		foreach (var member in Members!)
		{
			console.WriteLine(member.Id + CursorPosition(selection + 3, 5) + member.Name);
			selection++;
		}

		console.Write("Select the member or press Backspace to get back.\n\n");
		selection = 0;
		console.Write(Blue + CursorPosition(3, 1) + Members[selection].Id +
		              CursorPosition(3, 5) + Members[selection].Name + Reset);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						console.Write(ToLineStart + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name);
						selection--;
						console.Write(LineUp + ToLineStart + Blue + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name + Reset);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selection < Members.Count - 1)
					{
						console.Write(ToLineStart + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name);
						selection++;
						console.Write(LineDown + ToLineStart + Blue + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name + Reset);
					}

					break;
				case ConsoleKey.Enter:
					return Members[selection];
				case ConsoleKey.Backspace:
					return null;
			}
	}

	private void PrintRow(Book? book, int row, string color)
	{
		console.Write(color);
		console.WriteLine(CursorPosition(row, 1) + book?.Id + CursorPosition(row, 5) +
		                  (book?.Title.Length > 30 ? book.Title.Substring(0, 30) + "..." : book?.Title) +
		                  CursorPosition(row, 40) +
		                  (book?.Author.Length > 25 ? book.Author.Substring(0, 22) + "..." : book?.Author) +
		                  CursorPosition(row, 68) + book?.MemberName + CursorPosition(row, 93) +
		                  book?.BorrowedDate?.ToShortDateString() + Reset);
	}
}