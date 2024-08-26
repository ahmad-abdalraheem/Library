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
		book.Title = book.Title.Trim();
		book.Author = book.Author.Trim();
		if (book.Title.Length == 0 || book.Author.Length == 0)
			throw new ArgumentException("Title and Author are required and cannot be empty");

		context.Books.Update(book);
		context.SaveChanges();
		return book;
	}

	public bool Delete(int bookId)
	{
		Book? book = context.Books?.FirstOrDefault(b => b.Id == bookId);
		if (book == null)
			throw new KeyNotFoundException(message: "No books found with id : " + bookId);

		context.Books?.Remove(book);
		context.SaveChanges();
		return true;
	}

	public List<Book>? Get()
	{
		try
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
					BorrowedBy = book.BorrowedBy,
					Borrower = member,
					MemberName = member != null ? member.Name : null
				};

			return query.ToList();
		}
		catch (Exception e)
		{
			Console.WriteLine("\x1b[41mError \x1b[0m>Book Repo > Get > " + e.Message);
			throw;
		}
	}

	public Book? GetById(int bookId)
	{
		try
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
					BorrowedBy = book.BorrowedBy,
					MemberName = member != null ? member.Name : null
				}).FirstOrDefault();

			return query;
		}
		catch (Exception e)
		{
			Console.WriteLine("\x1b[41mError \x1b[0m>Book Repo > GetById > " + e.Message);
			throw;
		}
	}
}