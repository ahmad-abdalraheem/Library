using Domain.Entities;
using Domain.Repository;

namespace Application.Repository;

public class BookRepository(LibraryContext context) : IBookRepository
{
	public Book Add(Book book)
	{
		context.Books.Add(book);
		context.SaveChanges();
		return book;
	}

	public Book Update(Book book)
	{
		context.Books.Update(book);
		context.SaveChanges();
		return book;
	}

	public bool Delete(int bookId)
	{
		Book? book = context.Books.FirstOrDefault(b => b.Id == bookId);
		if (book == null)
			throw new KeyNotFoundException(message: "No books found with id : " + bookId);

		context.Books.Remove(book);
		context.SaveChanges();
		return true;
	}

	public List<Book> Get()
	{
		var query = from book in context.Books
			join member in context.Members
				on book.BorrowedBy equals member.Id into bookMembers
			from member in bookMembers.DefaultIfEmpty()
			select new Book
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author,
				IsBorrowed = book.IsBorrowed,
				BorrowedDate = book.BorrowedDate, 
				Borrower = member
			};

		return query.ToList();
	}

	public Book GetById(int bookId)
	{
		Book? query = (from book in context.Books
			where book.Id == bookId
			join member in context.Members
				on book.BorrowedBy equals member.Id into bookMembers
			from member in bookMembers.DefaultIfEmpty()
			select new Book
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author,
				IsBorrowed = book.IsBorrowed,
				BorrowedDate = book.BorrowedDate,
				BorrowedBy = book.BorrowedBy
			}).FirstOrDefault();

		if (query == null)
			throw new KeyNotFoundException("No books found with id : " + bookId);

		return query;
	}
}