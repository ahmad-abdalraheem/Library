using Domain.Entities;

namespace Infrastructure.DataHandler;

public interface IDataHandler<T> where T : IEntity
{
	bool Write(List<T> entities);
	List<T>? Read();
}