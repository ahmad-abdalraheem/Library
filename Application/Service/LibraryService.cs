using Domain.Entities;

namespace Application.Service;

public class LibraryService(BookService bookService, MemberService memberService)
{
	private List<Book>? _books = bookService.Get();
	private List<Member>? _members = memberService.Get();

	public virtual List<Book>? GetBorrowed()
	{
		var borrowedBooks = (_books ?? bookService.Get())?.FindAll(b => b.IsBorrowed);
		_members ??= memberService.Get();
		if (borrowedBooks != null)
			foreach (var book in borrowedBooks)
				book.MemberName = _members?.Find(m => m.Id == book.BorrowedBy)?.Name ?? "unknown";
		return borrowedBooks;
	}

	public virtual List<Book>? GetAvailable()
	{
		return (bookService.Get())?.FindAll(b => b.IsBorrowed == false);
	}

	public virtual bool ReturnBook(int bookId)
	{
		_books ??= bookService.Get();
		Book? book = null;
		if ((_books == null || _books.Count == 0) || (book ??= _books?.Find(b => b.Id == bookId)) == null)
			return false;
		book.IsBorrowed = false;
		book.BorrowedBy = null;
		book.BorrowedDate = null;
		book.Borrower = null;
		return bookService.Update(book);
	}

	public virtual bool BorrowBook(int bookId, int memberId)
	{
		_books ??= bookService.Get();
		if (_books == null || _books.Count == 0)
			return false;
		if (memberService.GetById(memberId) == null)
			return false;
		Book? book = _books.Find(b => b.Id == bookId);
		if (book == null)
			return false;
		book.IsBorrowed = true;
		book.Borrower = _members?.Find(m => m.Id == memberId);
		book.BorrowedBy = memberId;
		book.BorrowedDate = DateOnly.FromDateTime(DateTime.Today);
		_books[_books.FindIndex(b => b.Id == book.Id)] = book;

		return bookService.Update(book);
	}
}