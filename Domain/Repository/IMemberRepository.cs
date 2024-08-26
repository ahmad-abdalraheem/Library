using Domain.Entities;

namespace Domain.Repository;

public interface IMemberRepository
{
	public Member Add(Member member);
	public Member Update(Member member);
	public bool Delete(int memberId);
	public List<Member>? Get();
	public Member? GetById(int memberId);
}