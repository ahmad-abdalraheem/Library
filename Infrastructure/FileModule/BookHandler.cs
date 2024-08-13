using System.Text.Json;
using Domain.Entities;
using Domain.FileHandler;

namespace Infrastructure.FileModule;

public class BookHandler : IBookHandler
{
	private readonly string _filePath =
		"/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Books.json";

	public bool Write(List<Book> books)
	{
		try
		{
			if (File.Exists(_filePath) == false)
				throw new FileNotFoundException();
			
			var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_filePath, json);
			return true;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return false;
		}
	}

	public List<Book>? Read()
	{
		try
		{
			if (File.Exists(_filePath) == false)
				throw new FileNotFoundException();
			
			var json = File.ReadAllText(_filePath);

			return JsonSerializer.Deserialize<List<Book>>(json);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return null;
		}
	}
}