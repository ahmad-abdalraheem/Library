using Domain.Entities;

namespace Application.Service;

public class LibraryService(BookService bookService, MemberService memberService)
{
	private List<Book>? _books;
	private List<Member>? _members;

	public List<Book>? GetBorrowed()
	{
			var borrowedBooks = (_books ?? bookService.Get())?.FindAll(b => b.IsBorrowed);
			_members ??= memberService.Get();
			if (borrowedBooks != null)
				foreach (var book in borrowedBooks)
					book.MemberName = _members?.Find(m => m.Id == book.BorrowedBy)?.Name ?? "unknown";
			return borrowedBooks;
	}

	public List<Book>? GetAvailable()
	{
		return (_books ?? bookService.Get())?.FindAll(b => b.IsBorrowed == false);
	}

	public bool ReturnBook(int bookId)
	{
		_books ??= bookService.Get();
		if (_books == null)
			return false;
		var book = _books.Find(b => b.Id == bookId);
		if (book == null)
			return false;
		book.IsBorrowed = false;
		book.BorrowedBy = null;
		book.BorrowedDate = null;
		return bookService.Update(book);
	}

	public bool BorrowBook(Book book, Member member)
	{
		_books ??= bookService.Get();
		if (_books == null)
			return false;
		book.IsBorrowed = true;
		book.BorrowedBy = member.Id;
		book.BorrowedDate = DateTime.Now;
		_books[_books.FindIndex(b => b.Id == book.Id)] = book;

		return bookService.Update(book);
	}
}