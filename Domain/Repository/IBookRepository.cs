using Domain.Entities;

namespace Domain.Repository;

public interface IBookRepository
{
	public Book Add(Book book);
	public Book Update(Book book);
	public bool Delete(int bookId);
	public List<Book> Get();
	public Book? GetById(int bookId);
}