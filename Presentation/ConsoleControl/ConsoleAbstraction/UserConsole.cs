namespace ConsoleApp;

public class UserConsole : IConsole
{
	public string? ReadLine(string? output = null) => Console.ReadLine();
	
	public ConsoleKey ReadKey(ConsoleKeyInfo? key = null) => Console.ReadKey(true).Key;
	public void Write(string value) => Console.Write(value);
	public void WriteLine() => Console.WriteLine();
	public void WriteLine(string value) => Console.WriteLine(value);
	public void Clear() => Console.Clear();
	
}