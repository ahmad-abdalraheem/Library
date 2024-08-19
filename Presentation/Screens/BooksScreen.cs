using System.Diagnostics;
using Application.Service;
using Domain.Entities;
using static ConsoleApp.Ansi;

namespace ConsoleApp;

public class BooksScreen(BookService bookService, IConsole console)
{
	private List<Book>? _books = bookService.Get();

	public int BooksMenu()
	{
		if ((_books ??= bookService.Get()) == null)
		{
			console.WriteLine(Red + "Error While loading Data." + Reset);
			console.ReadKey();
			return 0;
		}

		var isExit = false;
		while (!isExit)
		{
			console.Clear();
			if (_books?.Count == 0)
			{
				console.WriteLine(Red + "No Books found." + Reset);
				switch (UserInteraction.GetUserSelection(["Add a new book", "Back to main menu."], console))
				{
					case 0:
						bookService.Add(AddBook());
						_books = bookService.Get();
						break;
					default:
						isExit = true;
						break;
				}
			}
			else
			{
				DisplayBooks();
				isExit = BooksOperation();
			}
		}

		return 0;
	}
	private void DisplayBooks()
	{
		console.Clear();

		var currentRow = 1;
		console.Write($"ID{CursorPosition(1, 5)}Title{CursorPosition(1, 40)}Author" +
		              $"{CursorPosition(1, 67)}Status{CursorPosition(1, 79)}");
		console.Write("\n______________________________________________________________\n" + Reset);
		currentRow += 2;
		if (_books != null)
			foreach (var book in _books)
			{
				PrintRow(book, currentRow++, Reset);
				console.WriteLine();
			}

		console.WriteLine(Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		console.WriteLine("- Delete Key -> Delete selected record.");
		console.WriteLine("- Enter Key -> Update selected record.");
		console.WriteLine("- Plus (+) Key -> Add a new record.");
		console.WriteLine("- Backspace Key -> Get back to Main Menu." + Reset);
	}
	private bool BooksOperation()
	{
		var selected = 0;
		PrintRow(_books?[selected], 3, Blue);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selected > 0)
					{
						PrintRow(_books?[selected], selected + 3, Reset);
						selected--;
						PrintRow(_books?[selected], selected + 3, Blue);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selected < _books?.Count - 1)
					{
						PrintRow(_books?[selected], selected + 3, Reset);
						selected++;
						PrintRow(_books?[selected], selected + 3, Blue);
					}

					break;
				case ConsoleKey.Enter:
					if (_books != null)
					{
						_books[selected] = UpdateBook(_books[selected]);
						bookService.Update(_books[selected]);
						_books = bookService.Get();
					}
					return false;
				case ConsoleKey.Delete:
					if (_books != null) // always true, just for warning.
						bookService.Delete(_books[selected].Id);
					
					_books = bookService.Get();
					return false;
				case ConsoleKey.Add:
					bookService.Add(AddBook());
					_books = bookService.Get();
					return false;
				case ConsoleKey.Backspace:
					return true;
			}
	}
	private Book AddBook()
	{
		console.Clear();
		Book book = new Book("NA", "NA")
		{
			Title = "NA", 
			Author = "NA",
			IsBorrowed = false,
			BorrowedBy = null,
			BorrowedDate = null
		};

		console.Write(Yellow + "Book Title : " + Reset + ShowCursor);
		string? input = console.ReadLine();
		while(input?.Trim().Length == 0)
		{
			console.Write(Red + "Book Title Cannot be empty." + LineUp + ToLineStart);
			console.Write(Yellow + "Book Title : " + Reset + ShowCursor);
			input = console.ReadLine();
		}
		book.Title = input ?? "undefined";
		
		console.Write(Yellow + ClearLine + "Book Author : " + Reset + ShowCursor);
		input = console.ReadLine();
		while(input?.Trim().Length == 0)
		{
			console.Write(Red + "Book Author Cannot be empty." + LineUp + ToLineStart);
			console.Write(Yellow + "Book Author : " + Reset + ShowCursor);
			input = console.ReadLine();
		}
		book.Author = input ?? "undefined";
		console.Write(HideCursor);
		
		return book;
	}
	private Book UpdateBook(Book book)
	{
		console.Clear();
		console.WriteLine(Yellow + "Book ID : " + Reset + book.Id);
		console.Write(Yellow + "Book Title : " + Reset);
		string input = console.ReadLine()?.Trim() ?? "";
		book.Title = input == string.Empty ? book.Title : input;
		console.Write(LineUp + MoveRight(13) + book.Title + "\n");
		console.Write(Yellow + "Book Author : " + Reset);
		input = console.ReadLine()?.Trim() ?? "";
		book.Author = input == string.Empty ? book.Author : input;
		console.Write(LineUp + MoveRight(14) + book.Author + "\n");
		return book;
	}
	private void PrintRow(Book? book, int row, string color)
	{
		console.Write(color);
		console.Write(CursorPosition(row, 1) + book?.Id + CursorPosition(row, 5) +
		              (book?.Title.Length > 30 ? book.Title.Substring(0, 30) + "..." : book?.Title) +
		              CursorPosition(row, 40) +
		              (book?.Author.Length > 25 ? book.Author.Substring(0, 22) + "..." : book?.Author) +
		              CursorPosition(row, 68) +
		              ((bool) book?.IsBorrowed ? $"{Red}Borrowed" : $"{Green}Available")
		              + CursorPosition(row, 79) + color +
		              (book.BorrowedDate != null ? book.BorrowedDate.Value.ToShortDateString() : "***"));
		console.Write(Reset);
	}
}