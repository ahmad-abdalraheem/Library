namespace ConsoleApp;

/// <summary>
/// This class will be used in testing instead of read Console  Class
/// </summary>
public class TestConsole : IConsole
{
	public Queue<string> Input { get; set; } = new();
	public Queue<ConsoleKey> KeyInput { get; set; } = new();
	public Queue<string> Output { get; } = new();
	public void AddInputSequence(List<string> inputs)
	{
		foreach (string input in inputs)  Input.Enqueue(input);
	}
	public void AddKeySequence(List<ConsoleKey> keys)
	{
		foreach (ConsoleKey key in keys) KeyInput.Enqueue(key);
	}
	public string ReadLine(string? output = null) => Input.Dequeue();
	public ConsoleKey ReadKey(ConsoleKeyInfo? key = null) => KeyInput.Dequeue();
	public void Write(string value) => Output.Enqueue(value);
	public void WriteLine() => Output.Enqueue(Environment.NewLine);
	public void WriteLine(string value) => Output.Enqueue(value + Environment.NewLine);
	
	public void Clear() => Console.Clear();
	
}