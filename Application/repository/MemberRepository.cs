using Domain.Entities;
using Domain.Repository;
using Infrastructure.DataHandler;

namespace Application.Repository;

public class MemberRepository(IDataHandler<Member> memberHandler) : IMemberRepository
{
	private List<Member>? _members;

	public bool Add(Member member)
	{
		if ((_members ?? Get()) == null)
			throw new FailWhileLoadingDataException();
		
		if (_members != null)
		{
			member.Name = member.Name.Trim().Length == 0 ? "Undefined" : member.Name.Trim();
			member.Email = member.Email?.Trim().Length == 0 ? "Undefined" : member.Email?.Trim();
			_members.Add(member);
		}
		return _members != null && memberHandler.Write(_members);
	}

	public bool Update(Member member)
	{
		if ((_members ?? Get()) == null)
			throw new FailWhileLoadingDataException();
		
		if (_members != null) // Always True, added to remove warning
		{
			var index = _members.FindIndex(m => m.Id == member.Id);
			_members[index] = member;
		}

		return _members != null && memberHandler.Write(_members);
	}

	public bool Delete(int memberId)
	{
		if ((_members ?? Get()) == null)
			throw new FailWhileLoadingDataException();
		
		_members?.Remove(_members.Find(m => m.Id == memberId)!);
		return _members != null && memberHandler.Write(_members);
	}

	public List<Member>? Get() => _members ??= memberHandler.Read();

	public Member? GetById(int memberId) => (_members ?? Get())?.Find(m => m.Id == memberId);
}