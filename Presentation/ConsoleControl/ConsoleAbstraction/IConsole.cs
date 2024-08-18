namespace ConsoleApp;

public interface IConsole
{
	public string? ReadLine(string? output = null);
	public ConsoleKey ReadKey(ConsoleKeyInfo? key = null);
	public void Write(string value);
	public void WriteLine();
	public void WriteLine(string value);
	
	public void Clear();
}