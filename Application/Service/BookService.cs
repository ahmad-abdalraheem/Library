using Domain.Entities;
using Domain.Repository;

namespace Application.Service;

public sealed class BookService(IBookRepository bookRepository)
{
	private readonly IBookRepository _bookRepository =
		bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

	public Book Add(Book book)
	{
		if (book == null)
			throw new ArgumentNullException(nameof(book), "Book cannot be null.");

		book.Title = book.Title.Trim();
		book.Author = book.Author.Trim();
		if (book.Title.Length == 0 || book.Author.Length == 0)
			throw new ArgumentException("Title and Author are required and cannot be empty");

		book.Id = 0;
		book.IsBorrowed = false;

		return _bookRepository.Add(book);
	}

	public Book Update(Book book)
	{
		book.Title = book.Title.Trim();
		book.Author = book.Author.Trim();
		if (book.Title.Length == 0 || book.Author.Length == 0)
			throw new ArgumentException("Title and Author are required and cannot be empty");

		return _bookRepository.Update(book);
	}

	public bool Delete(int bookId)
	{
		if (bookId <= 0)
			throw new IndexOutOfRangeException("Book Id cannot be negative.");

		return _bookRepository.Delete(bookId);
	}

	public List<Book>? Get()
	{
		return _bookRepository.Get();
	}

	public Book? GetById(int bookId)
	{
		if (bookId <= 0)
			throw new IndexOutOfRangeException("Book Id cannot be negative.");
		
		return _bookRepository.GetById(bookId);
	}
}