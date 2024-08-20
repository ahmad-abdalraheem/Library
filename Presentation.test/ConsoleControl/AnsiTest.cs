using ConsoleApp;

namespace Presentation.test.ConsoleControl;

public class AnsiTest
{
	[Fact]
	public void ClearCommands()
	{
		Assert.Equal("\x1b[2J", Ansi.Clear);
		Assert.Equal("\x1b[2K", Ansi.ClearLine);
	}

	[Fact]
	public void TextStylingCommands()
	{
		Assert.Equal("\x1b[8m", Ansi.HideText);
		Assert.Equal("\x1b[0m", Ansi.Reset);
		Assert.Equal("\x1b[4m", Ansi.UnderLine);
		Assert.Equal("\x1b[1m", Ansi.Bold);
		Assert.Equal("\x1b[3m", Ansi.Italic);
	}

	[Fact]
	public void CursorMovementCommands()
	{
		Assert.Equal("\x1b[1A\x1b[1G", Ansi.LineUp);
		Assert.Equal("\x1b[1B\x1b[1G", Ansi.LineDown);
		Assert.Equal("\x1b[1G", Ansi.ToLineStart);
	}

	[Fact]
	public void MoveRight_ShouldReturnString_WhenInputIsRight()
	{
		Assert.Equal("\x1b[1C", Ansi.MoveRight(1));
		Assert.Equal("\x1b[10C", Ansi.MoveRight(10));
		Assert.Equal("\x1b[5C", Ansi.MoveRight(5));
	}

	[Fact]
	public void MoveLeft_ShouldReturnString_WhenInputIsRight()
	{
		Assert.Equal("\x1b[1D", Ansi.MoveLeft(1));
		Assert.Equal("\x1b[10D", Ansi.MoveLeft(10));
		Assert.Equal("\x1b[5D", Ansi.MoveLeft(5));
	}

	[Fact]
	public void CursorPositioningCommands()
	{
		Assert.Equal("\x1b[s", Ansi.SavePosition);
		Assert.Equal("\x1b[u", Ansi.RestorePosition);
	}

	[Fact]
	public void CursorPosition_ShouldReturnString_WhenInputIsRight()
	{
		for (int i = 3; i <= 30; i *= 3)
		{
			for (int j = 1; j <= 4; j++)
			{
				Assert.Equal($"\x1b[{i};{j}H", Ansi.CursorPosition(i, j));
			}
		}
	}

	[Fact]
	public void ForegroundColorCommands()
	{
		Assert.Equal("\x1b[31m", Ansi.Red);
		Assert.Equal("\x1b[32m", Ansi.Green);
		Assert.Equal("\x1b[33m", Ansi.Yellow);
		Assert.Equal("\x1b[34m", Ansi.Blue);
		Assert.Equal("\x1b[35m", Ansi.Magenta);
		Assert.Equal("\x1b[36m", Ansi.Cyan);
	}

	[Fact]
	public void CursorControlCommands()
	{
		Assert.Equal("\x1b[?25l", Ansi.HideCursor);
		Assert.Equal("\x1b[?25h", Ansi.ShowCursor);
	}
}