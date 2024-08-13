using Domain.Entities;

namespace Domain.Repository;

public interface IMemberRepository
{
	public bool Add(Member member);
	public bool Update(Member member);
	public bool Delete(int memberId);
	public List<Member>? Get();
	public Member? GetById(int memberId);
}