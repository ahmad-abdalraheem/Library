using Domain.Entities;
using Domain.Repository;

namespace Application.Repository;

public class MemberRepository(LibraryContext context) : IMemberRepository
{
	public Member Add(Member member)
	{
		context.Members.Add(member);
		context.SaveChanges();
		return member;
	}

	public Member Update(Member member)
	{
		context.Members.Update(member);
		context.SaveChanges();
		return member;
	}

	public bool Delete(int memberId)
	{
		Member? member = context.Members.Find(memberId);
		if (member == null)
			throw new KeyNotFoundException(message: "No members found with Id: " + memberId);

		context.UpdateIsBorrowedStatus(member);
		context.Members.Remove(member);
		context.SaveChanges();
		return true;
	}

	public List<Member> Get()
	{
		return context.Members.ToList();
	}

	public Member GetById(int memberId)
	{
		Member? member = context.Members.Find(memberId);
		if (member == null)
			throw new KeyNotFoundException(message: "No members found with Id: " + memberId);

		return member;
	}
}