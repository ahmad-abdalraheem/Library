using Application.Service;
using Domain.Entities;

namespace ConsoleApp;

public class BooksScreen(BookService bookService, IConsole console)
{
	private List<Book>? _books = bookService.Get();

	public int BooksMenu()
	{
		if (_books == null && (_books = bookService.Get()) == null)
			return 0;

		var isExit = false;
		while (!isExit)
		{
			console.Clear();
			if (_books?.Count == 0)
			{
				console.WriteLine(Ansi.Red + "No Books found." + Ansi.Reset);
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
		if (_books?.Count == 0)
			return;
		var currentRow = 1;
		console.Write($"ID{Ansi.CursorPosition(1, 5)}Title{Ansi.CursorPosition(1, 40)}Author" +
		              $"{Ansi.CursorPosition(1, 67)}Status{Ansi.CursorPosition(1, 79)}");
		console.Write("\n______________________________________________________________\n" + Ansi.Reset);
		currentRow += 2;
		if (_books != null)
			foreach (var book in _books)
			{
				PrintRow(book, currentRow++, Ansi.Reset);
				console.WriteLine();
			}

		console.WriteLine(Ansi.Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		console.WriteLine("- Delete Key -> Delete selected record.");
		console.WriteLine("- Enter Key -> Update selected record.");
		console.WriteLine("- Plus (+) Key -> Add a new record.");
		console.WriteLine("- Backspace Key -> Get back to Main Menu." + Ansi.Reset);
	}
	private bool BooksOperation()
	{
		var selected = 0;
		PrintRow(_books?[selected], 3, Ansi.Blue);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selected > 0)
					{
						PrintRow(_books?[selected], selected + 3, Ansi.Reset);
						selected--;
						PrintRow(_books?[selected], selected + 3, Ansi.Blue);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selected < _books?.Count - 1)
					{
						PrintRow(_books?[selected], selected + 3, Ansi.Reset);
						selected++;
						PrintRow(_books?[selected], selected + 3, Ansi.Blue);
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
					if (_books != null)
					{
						bookService.Delete(_books[selected].Id);
					}
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
		Book book = new Book
		{
			Title = "NA", 
			Author = "NA",
			IsBorrowed = false,
			BorrowedBy = null,
			BorrowedDate = null
		};

		console.Write(Ansi.Yellow + "Book Title : " + Ansi.Reset + Ansi.ShowCursor);
		string? input = console.ReadLine();
		while(input?.Trim().Length == 0)
		{
			console.Write(Ansi.Red + "Book Title Cannot be empty." + Ansi.LineUp + Ansi.ToLineStart);
			console.Write(Ansi.Yellow + "Book Title : " + Ansi.Reset + Ansi.ShowCursor);
			input = console.ReadLine();
		}
		book.Title = input ?? "undefined";
		
		console.Write(Ansi.Yellow + Ansi.ClearLine + "Book Author : " + Ansi.Reset + Ansi.ShowCursor);
		input = console.ReadLine();
		while(input?.Trim().Length == 0)
		{
			console.Write(Ansi.Red + "Book Author Cannot be empty." + Ansi.LineUp + Ansi.ToLineStart);
			console.Write(Ansi.Yellow + "Book Author : " + Ansi.Reset + Ansi.ShowCursor);
			input = console.ReadLine();
		}
		book.Author = input ?? "undefined";
		console.Write(Ansi.HideCursor);
		
		return book;
	}
	private Book UpdateBook(Book book)
	{
		console.Clear();
		console.WriteLine(Ansi.Yellow + "Book ID : " + Ansi.Reset + book.Id);
		console.Write(Ansi.Yellow + "Book Title : " + Ansi.Reset);
		string input = console.ReadLine()?.Trim() ?? "";
		book.Title = input == string.Empty ? book.Title : input;
		console.Write(Ansi.LineUp + Ansi.MoveRight(13) + book.Title + "\n");
		console.Write(Ansi.Yellow + "Book Author : " + Ansi.Reset);
		input = console.ReadLine()?.Trim() ?? "";
		book.Author = input == string.Empty ? book.Author : input;
		console.Write(Ansi.LineUp + Ansi.MoveRight(14) + book.Author + "\n");
		return book;
	}
	private void PrintRow(Book? book, int row, string color)
	{
		console.Write(color);
		console.Write(Ansi.CursorPosition(row, 1) + book?.Id + Ansi.CursorPosition(row, 5) +
		              (book?.Title.Length > 30 ? book.Title.Substring(0, 30) + "..." : book?.Title) +
		              Ansi.CursorPosition(row, 40) +
		              (book?.Author.Length > 25 ? book.Author.Substring(0, 22) + "..." : book?.Author) +
		              Ansi.CursorPosition(row, 68) +
		              ((bool) book?.IsBorrowed ? $"{Ansi.Red}Borrowed" : $"{Ansi.Green}Available")
		              + Ansi.CursorPosition(row, 79) + color +
		              (book.BorrowedDate != null ? book.BorrowedDate.Value.ToShortDateString() : "***"));
		console.Write(Ansi.Reset);
	}
}