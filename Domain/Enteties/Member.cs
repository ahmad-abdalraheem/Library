namespace Domain.Entities;

public class Member
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Email { get; set; }
}