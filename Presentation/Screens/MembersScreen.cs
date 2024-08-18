using Application.Service;
using Domain.Entities;
using static ConsoleApp.Ansi;

namespace ConsoleApp;

public class MembersScreen(MemberService memberService, IConsole console)
{
	private List<Member>? _members = memberService.Get();

	public int MembersMenu()
	{
		console.Clear();
		if (_members == null && (_members = memberService.Get()) == null)
		{
			console.WriteLine(Red + "Error While loading Data." + Reset);
			console.ReadKey();
			return 0;
		}

		var isExit = false;
		while (!isExit)
		{
			if (_members == null)
				return 0;
			console.Clear();
			if (_members?.Count == 0)
			{
				console.WriteLine(Red + "No members found." + Reset);
				switch (UserInteraction.GetUserSelection(["Add a new Member", "Back to main menu."], console))
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
		console.Write(CursorPosition(1, 1) + Clear + Yellow);
		console.Write($"ID{CursorPosition(1, 5)}Name{CursorPosition(1, 30)}Email");
		console.Write("\n__________________________________________________\n" + Reset);
		currentRow += 2;
		if (_members != null)
			foreach (var member in _members)
				PrintRowL(member, currentRow++);

		console.WriteLine(Yellow + "\nUse Arrow (Up/Down) To select Record, then press:");
		console.WriteLine("- Delete Key -> Delete selected record.");
		console.WriteLine("- Enter Key -> Update selected record.");
		console.WriteLine("- Plus (+) Key -> Add a new record.");
		console.WriteLine("- Backspace Key -> Get back to Main Menu." + Reset);
	}
	private bool MemberOperation()
	{
		var selected = 0;
		PrintRowL(_members![selected], 3, Blue);
		while (true)
			switch (console.ReadKey())
			{
				case ConsoleKey.UpArrow:
					if (selected > 0)
					{
						PrintRow(_members[selected], selected + 3);
						selected--;
						PrintRow(_members[selected], selected + 3, Blue);
					}

					break;
				case ConsoleKey.DownArrow:
					if (selected < _members.Count - 1)
					{
						PrintRowL(_members[selected], selected + 3);
						selected++;
						PrintRowL(_members[selected], selected + 3, Blue);
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
		console.Clear();
		var member = new Member { Name = "" };
		console.Write(Yellow + "Member Name : " + Reset + ShowCursor);
		string? input = console.ReadLine();
		while (input?.Trim().Length == 0)
		{
			console.Write(Red + "Name Cannot be empty." + LineUp + ToLineStart + ClearLine);
			console.Write(Yellow + "Member Name : " + Reset + ShowCursor);
			input = console.ReadLine();
		}
		member.Name = input ?? "undefined";
		
		console.Write(ClearLine + Yellow + "Email : " + Reset);
		member.Email = console.ReadLine()?.Trim();
		console.Write(HideCursor);
		return member;
	}
	private Member UpdateMember(Member member)
	{
		console.Clear();
		console.WriteLine(Yellow + "Member ID : " + Reset + member.Id);
		console.Write(Yellow + "Member Name : " + Reset);
		string? input = console.ReadLine()?.Trim() ?? "";
		member.Name = input == string.Empty ? member.Name : input;
		console.Write(LineUp + MoveRight(14) + member.Name + "\n");
		console.Write(Yellow + "Member Email : " + Reset);
		input = console.ReadLine()?.Trim();
		member.Email = input == string.Empty ? member.Email : input;
		return member;
	}
	private void PrintRow(Member member, int row, string color = "\x1b[0m")
	{
		console.Write(color + CursorPosition(row, 1) + member.Id + CursorPosition(row, 5) + member.Name +
		              CursorPosition(row, 30) + member.Email + Reset);
	}
	private void PrintRowL(Member member, int row, string color = "\x1b[0m")
	{
		console.WriteLine(color + CursorPosition(row, 1) + member.Id + CursorPosition(row, 5) + member.Name +
		                  CursorPosition(row, 30) + member.Email + Reset);
	}
}