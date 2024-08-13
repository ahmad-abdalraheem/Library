using Domain.Entities;
using Domain.Repository;
using static System.Console;

namespace Application.Service;

public class BookService(IBookRepository bookRepository)
{
	private readonly IBookRepository _bookRepository =
		bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

	public bool Add(Book book)
	{
		try
		{
			return _bookRepository.Add(book);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	public bool Update(Book book)
	{
		try
		{
			return _bookRepository.Update(book);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	public bool Delete(int bookId)
	{
		try
		{
			return _bookRepository.Delete(bookId);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	public List<Book>? Get()
	{
		try
		{
			return _bookRepository.Get();
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return null;
		}
	}
}