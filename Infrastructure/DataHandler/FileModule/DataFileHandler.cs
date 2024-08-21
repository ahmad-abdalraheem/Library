using System.Text.Json;
using Domain.Entities;

namespace Infrastructure.DataHandler;

public class DataFileHandler<T>(string filePath) : IDataHandler<T>
	where T : IEntity
{
	public bool Write(List<T> entities)
	{
		try
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException();

			var json = JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filePath, json);
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
			if (!File.Exists(filePath))
				throw new FileNotFoundException();

			var json = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<List<T>>(json);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return null;
		}
	}
}