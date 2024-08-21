using Domain.Entities;
using Domain.Repository;
using Infrastructure.DataHandler;

namespace Application.Repository;

public class BookRepository(IDataHandler<Book> bookHandler) : IBookRepository
{
	private List<Book>? _books;

	public bool Add(Book book)
	{
		if ((_books ?? Get()) == null)
			throw new FailWhileLoadingDataException("An Error Occurs While Retrieving Books Data");
		if (_books != null)
		{
			book.Title = book.Title.Trim().Length == 0 ? "Undefined" : book.Title.Trim();
			book.Author = book.Author.Trim().Length == 0 ? "Undefined" : book.Author.Trim();
			_books.Add(book);
		}

		return _books != null && bookHandler.Write(_books);
	}

	public bool Update(Book book)
	{
		if ((_books ?? Get()) == null)
			throw new FailWhileLoadingDataException("An Error Occurs While Retrieving Books Data");
		if (_books != null)
		{
			var index = _books.FindIndex(m => m.Id == book.Id);
			_books[index] = book;
		}

		return _books != null && bookHandler.Write(_books);
	}

	public bool Delete(int bookId)
	{
		if ((_books ?? Get()) == null)
			throw new FailWhileLoadingDataException("An Error Occurs While Retrieving Books Data");
		_books?.Remove(_books.Find(b => b.Id == bookId)!);
		return _books != null && bookHandler.Write(_books);
	}

	public List<Book>? Get() =>_books ??= bookHandler.Read();

	public Book? GetById(int bookId) => (_books ?? Get())?.Find(m => m.Id == bookId);
}