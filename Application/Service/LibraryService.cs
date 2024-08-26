using Domain.Entities;

namespace Application.Service;

public class LibraryService(BookService bookService, MemberService memberService)
{
	public virtual List<Book>? GetBorrowed() => bookService.Get()?.FindAll(b => b.IsBorrowed);

	public virtual List<Book>? GetAvailable() => bookService.Get()?.FindAll(b => b.IsBorrowed == false);

	public virtual Book ReturnBook(int bookId) 
	{
		Book? book = bookService.GetById(bookId);
		
		if(book == null)
			throw new KeyNotFoundException();
		
		book.IsBorrowed = false;
		book.Borrower = null;
		book.BorrowedDate = null;
		book.BorrowedBy = null;

		return bookService.Update(book);
	}

	public Book BorrowBook(int bookId, int memberId)
	{
		Book? book = bookService.GetById(bookId);
		Member? member = memberService.GetById(memberId);
		
		if(book == null || member == null)
			throw new KeyNotFoundException("Book or Member not found");
		
		book.IsBorrowed = true;
		book.Borrower = member;
		book.BorrowedBy = member.Id;
		book.BorrowedDate = DateOnly.FromDateTime(DateTime.Now);
		
		return bookService.Update(book);
	}
}