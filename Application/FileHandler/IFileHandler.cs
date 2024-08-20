using Domain.Entities;

namespace Application.FileHandler;

public interface IFileHandler<T> where T : IEntity
{
	bool Write(List<T> entities);
	List<T>? Read();
}