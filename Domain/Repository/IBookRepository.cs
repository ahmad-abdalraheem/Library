using Domain.Entities;

namespace Domain.Repository;

public interface IBookRepository
{
	public bool Add(Book book);
	public bool Update(Book? book);
	public bool Delete(int bookId);
	public List<Book?>? Get();
	public Book? GetById(int bookId);
}