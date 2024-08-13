using Domain.Entities;

namespace Domain.FileHandler;

public interface IBookHandler
{
	public bool Write(List<Book> books);
	public List<Book>? Read();
}