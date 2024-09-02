using AutoMapper;
using Domain.Entities;

namespace Application.Service;

public sealed class LibraryService(BookService bookService, MemberService memberService, IMapper mapper)
{
	public List<GetBookDto> GetBorrowed() => bookService.Get().FindAll(b => b.IsBorrowed);

	public List<GetBookDto> GetAvailable() => bookService.Get().FindAll(b => b.IsBorrowed == false);

	public GetBookDto ReturnBook(int bookId)
	{
		Book? book = mapper.Map<Book>(bookService.GetById(bookId));

		if (book == null)
			throw new KeyNotFoundException("No book found with Id : " + bookId);

		if (!book.IsBorrowed)
			throw new KeyNotFoundException("Book is not borrowed");

		book.IsBorrowed = false;
		book.Borrower = null;
		book.BorrowedDate = null;

		return bookService.Update(mapper.Map<UpdateBookDto>(book));
	}

	public GetBookDto BorrowBook(int bookId, int memberId)
	{
		Book? book = mapper.Map<Book>(bookService.GetById(bookId));
		Member? member = memberService.GetById(memberId);

		if (book == null)
			throw new KeyNotFoundException("No book found with Id : " + bookId);

		if (member == null)
			throw new KeyNotFoundException("No member found with Id : " + memberId);

		if (book.IsBorrowed)
			throw new KeyNotFoundException("Book is borrowed");

		book.IsBorrowed = true;
		book.Borrower = member;
		book.BorrowedDate = DateOnly.FromDateTime(DateTime.Now);

		return bookService.Update(mapper.Map<UpdateBookDto>(book));
	}
}