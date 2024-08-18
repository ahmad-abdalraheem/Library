using Domain.Entities;

namespace Application.Service;

public class LibraryService(BookService bookService,MemberService memberService)
{
	private List<Book>? Books = bookService.Get();
	private List<Member>? Members = memberService.Get();

	public List<Book>? GetBorrowed()
	{
			var borrowedBooks = (Books ?? bookService.Get())?.FindAll(b => b.IsBorrowed);
			Members ??= memberService.Get();
			if (borrowedBooks != null)
				foreach (var book in borrowedBooks)
					book.MemberName = Members?.Find(m => m.Id == book.BorrowedBy)?.Name ?? "unknown";
			return borrowedBooks;
	}

	public List<Book>? GetAvailable()
	{
		return (bookService.Get())?.FindAll(b => b.IsBorrowed == false);
	}

	public bool ReturnBook(int bookId)
	{
		Books ??= bookService.Get();
		if (Books == null)
			return false;
		var book = Books.Find(b => b.Id == bookId);
		if (book == null)
			return false;
		book.IsBorrowed = false;
		book.BorrowedBy = null;
		book.BorrowedDate = null;
		return bookService.Update(book);
	}

	public bool BorrowBook(Book book, Member member)
	{
		Books ??= bookService.Get();
		if (Books == null)
			return false;
		if (memberService.GetById(member.Id) == null)
			return false;
		book.IsBorrowed = true;
		book.BorrowedBy = member.Id;
		book.BorrowedDate = DateTime.Now;
		Books[Books.FindIndex(b => b.Id == book.Id)] = book;

		return bookService.Update(book);
	}
}