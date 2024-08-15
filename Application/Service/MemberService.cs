using Domain.Entities;
using Domain.Repository;
using static System.Console;

namespace Application.Service;

public class MemberService(IMemberRepository memberRepository)
{
	private readonly IMemberRepository _memberRepository =
		memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));

	public bool Add(Member member)
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

	public bool Update(Member member)
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

	public bool Delete(int memberId)
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

	public List<Member>? Get()
	{
		try
		{
			return _memberRepository.Get();
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return null;
		}
	}

	public Member? GetById(int memberId)
	{
		try
		{
			return _memberRepository.GetById(memberId);
		}
		catch (Exception e)
		{
			WriteLine(e.Message);
			return null;
		}
	}
}