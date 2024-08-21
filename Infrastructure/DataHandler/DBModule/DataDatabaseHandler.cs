using Domain.Entities;

namespace Infrastructure.DataHandler
{
	public class DataDatabaseHandler<T>(LibraryContext context) : IDataHandler<T>
		where T : class, IEntity
	{
		public bool Write(List<T> entities)
		{
			try
			{
				context.Set<T>().UpdateRange(entities);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
		}

		public List<T>? Read()
		{
			try
			{
				return context.Set<T>().ToList();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
	}
}