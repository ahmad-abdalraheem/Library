using Domain.Entities;
using Domain.Repository;
using static System.Console;

namespace Application.Service;

public class MemberService(IMemberRepository memberRepository)
{
	private readonly IMemberRepository _memberRepository =
		memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));

	public virtual bool Add(Member member)
	{
		try
		{
			return _memberRepository.Add(member);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	public virtual bool Update(Member member)
	{
		try
		{
			return _memberRepository.Update(member);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	public virtual bool Delete(int memberId)
	{
		try
		{
			return _memberRepository.Delete(memberId);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return false;
		}
	}

	// should I use try-catch? while member handler already catch the exception and return null
	public virtual List<Member>? Get() => _memberRepository.Get();

	public virtual Member? GetById(int memberId) => _memberRepository.GetById(memberId);
}