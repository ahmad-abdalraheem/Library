using Domain.Entities;
using Domain.Repository;
using static System.Console;

namespace Application.Service;

public class BookService(IBookRepository bookRepository)
{
	private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

	public virtual bool Add(Book book)
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

	public virtual bool Update(Book book)
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

	public virtual bool Delete(int bookId)
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
	public virtual List<Book>? Get()
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
	
	public virtual Book? GetById(int bookId)
	{
		try
		{
			return _bookRepository.GetById(bookId);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return null;
		}
	}
}