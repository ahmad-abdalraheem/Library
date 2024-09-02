using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Member : IEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	[Required]
	[MaxLength(50)]
	public required string Name { get; set; }
	
	[EmailAddress]
	[MaxLength(50)]
	public string? Email { get; set; }
}