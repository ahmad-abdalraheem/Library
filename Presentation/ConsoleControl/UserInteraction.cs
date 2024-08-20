namespace ConsoleApp;

public abstract class UserInteraction
{
	public static int GetUserSelection(
		List<string> options,
		IConsole console,
		string instruction = "Use Arrows(Up/Down) Then Enter to submit")
	{
		if (options.Count == 0) throw new InvalidDataException();
		var selection = 0;
		console.Write(Ansi.CursorPosition(2, 1));
		foreach (string option in options) console.WriteLine($">> {option}");
		console.WriteLine(Ansi.Yellow + instruction + Ansi.Reset);
		console.Write(Ansi.CursorPosition(2, 1) + Ansi.ClearLine + Ansi.Blue + ">> " + options[0] + Ansi.Reset +
		              Ansi.ToLineStart);

		var loopControl = true;
		while (loopControl)
		{
			ConsoleKey input = console.ReadKey();
			switch (input)
			{
				case ConsoleKey.UpArrow:
					if (selection > 0)
					{
						console.Write(Ansi.ClearLine + ">> " + options[selection]);
						selection--;
						console.Write(Ansi.LineUp + Ansi.ClearLine + Ansi.Blue + ">> " + options[selection] +
						              Ansi.Reset + Ansi.ToLineStart);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selection < options.Count - 1)
					{
						console.Write(Ansi.ClearLine + ">> " + options[selection]);
						selection++;
						console.Write(Ansi.LineDown + Ansi.ClearLine + Ansi.Blue + ">> " + options[selection] +
						              Ansi.Reset + Ansi.ToLineStart);
					}

					break;
				case ConsoleKey.Enter:
					loopControl = false;
					break;
			}
		}

		return selection;
	}
}