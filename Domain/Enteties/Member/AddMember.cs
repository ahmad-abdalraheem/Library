using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class AddMember
{
	[Required]
	[MaxLength(50)]
	public required string Name { get; set; }
	
	[EmailAddress]
	public string? Email { get; set; }	
}