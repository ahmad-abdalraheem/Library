using Application.Service;
using Domain.Entities;
using static ConsoleApp.Ansi;

namespace ConsoleApp;

public class BorrowScreen(LibraryService libraryService, MemberService memberService)
{
	private List<Book>? BorrowedBooks { get; set; }
	private List<Book?>? AvailableBooks { get; set; }
	private List<Member>? Members { get; set; }

	public int BorrowBookMenu()
	{
		Console.Clear();
		if ((BorrowedBooks ??= libraryService.GetBorrowed()) == null)
		{
			Console.WriteLine(Red + "Error While loading Data." + Reset);
			Console.ReadKey();
			return 0;
		}

		var isExit = false;
		while (!isExit)
			if (BorrowedBooks?.Count == 0)
			{
				Console.Clear();
				Console.Write(Red + "No Borrowed books. press on ADD button to borrow one or Backspace to get back." +
				              Reset);
				switch (UserInteraction.GetUserSelection(["Borrow new book.", "Get Back."]))
				{
					case 0:
						BorrowBook();
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
			switch (Console.ReadKey().Key)
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
		Console.Clear();
		if ((AvailableBooks ??= libraryService.GetAvailable()!) == null)
		{
			Console.WriteLine(Red + "Error while loading data" + Reset);
			Console.ReadKey();
			return;
		}

		if (Members == null && (Members = memberService.Get()) == null)
		{
			Console.WriteLine(Red + "Error while loading data" + Reset);
			Console.ReadKey();
			return;
		}

		var book = SelectAvailableBook();
		if (book == null)
			return;
		var member = SelectMember();
		if (member == null)
			return;
		libraryService.BorrowBook(book, member);
	}

	private void DisplayBorrowedBooks()
	{
		Console.Clear();
		if (BorrowedBooks?.Count == 0)
			return;
		var currentRow = 1;
		Console.Write($"ID{CursorPosition(1, 5)}Title{CursorPosition(1, 40)}Author" +
		              $"{CursorPosition(1, 68)}Borrowed By" +
		              $"{CursorPosition(1, 93)}Borrowed Date");
		Console.Write("\n______________________________________________________________\n" + Reset);
		currentRow += 2;
		if (BorrowedBooks != null)
			foreach (var book in BorrowedBooks)
				PrintRow(book, currentRow++, Reset);
		Console.WriteLine(Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		Console.WriteLine("- Enter Key -> Return the book");
		Console.WriteLine("- Plus (+) Key -> Borrow a new book");
		Console.WriteLine("- Backspace Key -> Get back to Main Menu." + Reset);
	}

	private Book? SelectAvailableBook()
	{
		Console.Write(Yellow + "ID   Title                              Author\n" +
		              "__________________________________________________________\n" + Reset);
		var selection = 0;
		foreach (var book in AvailableBooks!)
		{
			Console.Write(CursorPosition(selection + 3, 1) + book!.Id + CursorPosition(selection + 3, 5) +
			              (book.Title.Length > 30 ? book.Title.Substring(0, 30) : book.Title) +
			              CursorPosition(selection + 3, 40) + book.Author);
			selection++;
		}

		selection = 0;
		Console.WriteLine("\nSelect a book to borrow, Backspace to get back");
		Console.Write(Blue + CursorPosition(3, 1) + AvailableBooks[selection]!.Id + CursorPosition(3, 5) +
		              (AvailableBooks[selection]?.Title.Length > 30
			              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
			              : AvailableBooks[selection]?.Title)
		              + CursorPosition(3, 40) + AvailableBooks[selection]?.Author + Reset);
		while (true)
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						Console.Write(ToLineStart + AvailableBooks?[selection]?.Id + CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author);
						--selection;
						Console.Write(ToLineStart + Blue + AvailableBooks?[selection]?.Id +
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
						Console.Write(ToLineStart + AvailableBooks?[selection]?.Id + CursorPosition(selection + 3, 5) +
						              (AvailableBooks?[selection]?.Title.Length > 30
							              ? AvailableBooks[selection]?.Title.Substring(0, 30) + "..."
							              : AvailableBooks?[selection]?.Title)
						              + CursorPosition(selection + 3, 40) + AvailableBooks?[selection]?.Author);
						++selection;
						Console.Write(ToLineStart + Blue + AvailableBooks?[selection]?.Id +
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
		Console.Clear();
		Console.Write(Yellow + "ID     Name\n" + "______________________________\n" + Reset);
		var selection = 0;
		foreach (var member in Members!)
		{
			Console.WriteLine(member.Id + CursorPosition(selection + 3, 5) + member.Name);
			selection++;
		}

		Console.Write("Select the member or press Backspace to get back.\n\n");
		selection = 0;
		Console.Write(Blue + CursorPosition(3, 1) + Members[selection].Id +
		              CursorPosition(3, 5) + Members[selection].Name + Reset);
		while (true)
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						Console.Write(ToLineStart + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name);
						selection--;
						Console.Write(LineUp + ToLineStart + Blue + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name + Reset);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selection < Members.Count - 1)
					{
						Console.Write(ToLineStart + Members[selection].Id +
						              CursorPosition(selection + 3, 5) + Members[selection].Name);
						selection++;
						Console.Write(LineDown + ToLineStart + Blue + Members[selection].Id +
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
		Console.Write(color);
		Console.WriteLine(CursorPosition(row, 1) + book?.Id + CursorPosition(row, 5) +
		                  (book?.Title.Length > 30 ? book.Title.Substring(0, 30) + "..." : book?.Title) +
		                  CursorPosition(row, 40) +
		                  (book?.Author.Length > 25 ? book.Author.Substring(0, 22) + "..." : book?.Author) +
		                  CursorPosition(row, 68) + book?.MemberName + CursorPosition(row, 93) +
		                  book?.BorrowedDate?.ToShortDateString() + Reset);
	}
}