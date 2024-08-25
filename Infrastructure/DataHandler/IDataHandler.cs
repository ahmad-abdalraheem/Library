using Domain.Entities;

namespace Infrastructure.DataHandler;

public interface IDataHandler <TEntity> where TEntity : IEntity
{
	bool Add(TEntity entity);
	bool Update(TEntity entity);
	bool Delete(int entityId);
	List<TEntity>? Get();
	TEntity? GetById(int id);
	
}
