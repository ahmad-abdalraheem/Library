using ConsoleApp;

namespace Presentation.test.ConsoleControl;

public class UserInteractionTest
{
	[Fact]
	public void GetUserSelection_ShouldNotReturnNegative()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.UpArrow, ConsoleKey.UpArrow, ConsoleKey.Enter]);
		List<String> options = ["Option 1", "Option 2", "Option 3"];

		int selection = UserInteraction.GetUserSelection(options, console);
		
		Assert.Equal(0, selection);
	}
	
	[Fact]
	public void GetUserSelection_ShouldNotReturnOutOfRange()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
		List<String> options = ["Option 1", "Option 2", "Option 3"];
		string instruction = "Select one option";
		
		int selection = UserInteraction.GetUserSelection(options, console, instruction);
		
		Assert.Equal(options.Count - 1, selection);
	}

    [Fact]
    public void GetUserSelection_ShouldStartFromFirstOption()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([ConsoleKey.Enter]);
		List<String> options = ["Option 1", "Option 2", "Option 3"];
		string instruction = "Select one option";
		
		int selection = UserInteraction.GetUserSelection(options, console, instruction);
		
		Assert.Equal(0, selection);
	}

	[Fact]
	public void GetUserSelection_ShouldHandleAnyKey()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([ConsoleKey.G, ConsoleKey.Spacebar, ConsoleKey.Enter]);
		List<String> options = ["Option 1", "Option 2", "Option 3"];
		string instruction = "Select one option";
		
		int selection = UserInteraction.GetUserSelection(options, console, instruction);
		
		Assert.Equal(0, selection);	
	}

	[Fact]
	public void GetUserSelection_ShouldThrowException_WhenOptionIsNullOrEmpty()
	{
		TestConsole console = new TestConsole();
		console.AddKeySequence([ConsoleKey.G, ConsoleKey.Spacebar, ConsoleKey.Enter]);
		List<String> options = [];
		string instruction = "Select one option";
		
		Assert.ThrowsAny<InvalidDataException>(() => UserInteraction.GetUserSelection(options, console, instruction));
	}
}