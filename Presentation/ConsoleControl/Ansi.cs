namespace ConsoleApp;

/// <summary>
///     ANSI Escape Sequence used get control over console/terminal. Console.Write these strings to get affect
///     Some of these commands require specific position, it's implemented as a method instead of string
/// </summary>
public struct Ansi
{
	/// <summary>
	///     Clear Commands.
	/// </summary>
	public static string Clear = "\x1b[2J";

	public static string ClearLine = "\x1b[2K";

	/// <summary>
	///     Text Styling
	/// </summary>
	public static string HideText = "\x1b[8m";

	public static string Reset = "\x1b[0m";
	public static string UnderLine = "\x1b[4m";
	public static string Bold = "\x1b[1m";
	public static string Italic = "\x1b[3m";

	/// <summary>
	///     Cursor Movement
	/// </summary>
	public static string LineUp = "\x1b[1A\x1b[1G";

	public static string LineDown = "\x1b[1B\x1b[1G";

	public static string MoveRight(int steps = 1)
	{
		return $"\x1b[{steps}C";
	}

	public static string MoveLeft(int steps = 1)
	{
		return $"\x1b[{steps}D";
	}

	public static string ToLineStart = "\x1b[1G";

	/// <summary>
	///     Cursor Position
	/// </summary>
	public static string SavePosition = "\x1b[s";

	public static string RestorePosition = "\x1b[u";

	public static string CursorPosition(int row, int col)
	{
		return $"\x1b[{row};{col}H";
	}

	/// <summary>
	///     Foreground Colors
	/// </summary>
	public static string Black = "\x1b[30m";

	public static string Red = "\x1b[31m";
	public static string Green = "\x1b[32m";
	public static string Blue = "\x1b[34m";
	public static string Yellow = "\x1b[33m";
	public static string Cyan = "\x1b[36m";
	public static string Magenta = "\x1b[35m";

	/// <summary>
	///     Cursor Control
	/// </summary>
	public static string HideCursor = "\x1b[?25l";

	public static string ShowCursor = "\x1b[?25h";
}