using Domain.Entities;
using Domain.Repository;

namespace Application.Service;

public sealed class MemberService(IMemberRepository memberRepository)
{
	private readonly IMemberRepository _memberRepository =
		memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));

	public Member Add(Member member)
	{
		member.Id = 0;
		member.Name = member.Name.Trim();
		if (member.Name.Length == 0)
			throw new ArgumentException("Name cannot be empty");

		return _memberRepository.Add(member);
	}

	public Member Update(Member member)
	{
		member.Name = member.Name.Trim();
		if (member.Name.Length == 0)
			throw new ArgumentException("Name cannot be empty");

		return _memberRepository.Update(member);
	}

	public bool Delete(int memberId)
	{
		if (memberId <= 0)
			throw new IndexOutOfRangeException("Member Id cannot be negative or zero");

		return _memberRepository.Delete(memberId);
	}

	public List<Member>? Get() => _memberRepository.Get();

	public Member? GetById(int memberId)
	{
		if (memberId <= 0)
			throw new IndexOutOfRangeException("Member Id cannot be negative or zero");

		return _memberRepository.GetById(memberId);
	}
}