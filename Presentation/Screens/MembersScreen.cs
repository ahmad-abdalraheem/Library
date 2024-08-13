using Application.Service;
using Domain.Entities;

namespace ConsoleApp;

public class MembersScreen(MemberService memberService)
{
	private List<Member>? _members = memberService.Get();

	public int MembersMenu()
	{
		Console.Clear();
		if (_members == null && (_members = memberService.Get()) == null)
		{
			Console.WriteLine(Ansi.Red + "Error While loading Data." + Ansi.Reset);
			Console.ReadKey();
			return 0;
		}

		var isExit = false;
		while (!isExit)
		{
			if (_members == null)
				return 0;
			Console.Clear();
			if (_members?.Count == 0)
			{
				Console.WriteLine(Ansi.Red + "No members found." + Ansi.Reset);
				switch (UserInteraction.GetUserSelection(["Add a new Member", "Back to main menu."]))
				{
					case 0:
						memberService?.Add(AddMember());
						_members = memberService?.Get();
						break;
					default:
						isExit = true;
						break;
				}
			}
			else
			{
				DisplayMembers();
				isExit = MemberOperation();
			}
		}

		return 0;
	}

	private void DisplayMembers()
	{
		var currentRow = 1;
		Console.Write(Ansi.CursorPosition(1, 1) + Ansi.Clear + Ansi.Yellow);
		Console.Write($"ID{Ansi.CursorPosition(1, 5)}Name{Ansi.CursorPosition(1, 30)}Email");
		Console.Write("\n__________________________________________________\n" + Ansi.Reset);
		currentRow += 2;
		if (_members != null)
			foreach (var member in _members)
				PrintRowL(member, currentRow++);

		Console.WriteLine(Ansi.Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		Console.WriteLine("- Delete Key -> Delete selected record.");
		Console.WriteLine("- Enter Key -> Update selected record.");
		Console.WriteLine("- Plus (+) Key -> Add a new record.");
		Console.WriteLine("- Backspace Key -> Get back to Main Menu." + Ansi.Reset);
	}

	private bool MemberOperation()
	{
		var selected = 0;
		PrintRowL(_members![selected], 3, Ansi.Blue);
		while (true)
			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.UpArrow:
					if (selected > 0)
					{
						PrintRow(_members[selected], selected + 3);
						selected--;
						PrintRow(_members[selected], selected + 3, Ansi.Blue);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selected < _members.Count - 1)
					{
						PrintRowL(_members[selected], selected + 3);
						selected++;
						PrintRowL(_members[selected], selected + 3, Ansi.Blue);
					}

					break;
				case ConsoleKey.Enter:
					_members[selected] = UpdateMember(_members[selected]);
					memberService.Update(_members[selected]);
					_members = memberService.Get();
					return false;
				case ConsoleKey.Delete:
					memberService.Delete(_members[selected].Id);
					_members = memberService.Get();
					return false;
				case ConsoleKey.Add:
					memberService.Add(AddMember());
					_members = memberService.Get();
					return false;
				case ConsoleKey.Backspace:
					return true;
			}
	}

	private Member AddMember()
	{
		Console.Clear();
		var member = new Member { Name = "" };
		Console.Write(Ansi.Yellow + "Member Name : " + Ansi.Reset + Ansi.ShowCursor);
		member.Name = Console.ReadLine()?.Trim() ?? "Unknown";
		Console.Write(Ansi.ClearLine + Ansi.Yellow + "Email : " + Ansi.Reset);
		member.Email = Console.ReadLine()?.Trim();
		Console.Write(Ansi.HideCursor);
		return member;
	}

	private Member UpdateMember(Member member)
	{
		Console.Clear();
		Console.WriteLine(Ansi.Yellow + "Member ID : " + Ansi.Reset + member.Id);
		Console.Write(Ansi.Yellow + "Member Name : " + Ansi.Reset);
		string? input = Console.ReadLine()?.Trim() ?? "";
		member.Name = input == string.Empty ? member.Name : input;
		Console.Write(Ansi.LineUp + Ansi.MoveRight(14) + member.Name + "\n");
		Console.Write(Ansi.Yellow + "Member Email : " + Ansi.Reset);
		input = Console.ReadLine()?.Trim();
		member.Email = input == string.Empty ? member.Email : input;
		return member;
	}

	private void PrintRow(Member member, int row, string color = "\x1b[0m")
	{
		Console.Write(color + Ansi.CursorPosition(row, 1) + member.Id + Ansi.CursorPosition(row, 5) + member.Name +
		              Ansi.CursorPosition(row, 30) + member.Email + Ansi.Reset);
	}

	private void PrintRowL(Member member, int row, string color = "\x1b[0m")
	{
		Console.WriteLine(color + Ansi.CursorPosition(row, 1) + member.Id + Ansi.CursorPosition(row, 5) + member.Name +
		                  Ansi.CursorPosition(row, 30) + member.Email + Ansi.Reset);
	}
}