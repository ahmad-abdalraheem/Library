using Domain.Entities;

namespace Domain.FileHandler;

public interface IMemberHandler
{
	public bool Write(List<Member> members);
	public List<Member>? Read();
}