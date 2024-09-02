using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class AddBookDto
{
	[Required]
	[MaxLength(50)]
	public required string Title { get; set; }
	
	[Required]
	[MaxLength(50)]
	public required string Author { get; set; }
}