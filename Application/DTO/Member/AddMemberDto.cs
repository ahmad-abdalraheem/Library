using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class AddMemberDto
{
	[Required]
	[MaxLength(50)]
	public required string Name { get; set; }
	
	[EmailAddress]
	public string? Email { get; set; }	
}