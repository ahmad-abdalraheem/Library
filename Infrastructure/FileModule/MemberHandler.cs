using System.Text.Json;
using Domain.Entities;
using Domain.FileHandler;

namespace Infrastructure.FileModule;

public class MemberHandler : IMemberHandler
{
	private readonly string _filePath =
		"/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Members.json";

	public bool Write(List<Member> members)
	{
		try
		{
			if (File.Exists(_filePath) == false)
				throw new FileNotFoundException();
			
			string json = JsonSerializer.Serialize(members, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_filePath, json);
			return true;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return false;
		}
	}

	public List<Member>? Read()
	{
		try
		{
			if (File.Exists(_filePath) == false)
				throw new FileNotFoundException();
			
			string json = File.ReadAllText(_filePath);

			return JsonSerializer.Deserialize<List<Member>>(json);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return null;
		}
	}
}