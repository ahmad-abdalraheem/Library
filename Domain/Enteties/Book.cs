using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Book : IEntity
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	[Required]
	[MaxLength(50)]
	public required string Title { get; set; }
	
	[Required]
 	[MaxLength(50)]
	public required string Author { get; set; }
	
	public bool IsBorrowed { get; set; }
	
	public DateOnly? BorrowedDate { get; set; }
	
	public int? BorrowedBy { get; set; }
	
	[ForeignKey(name: "BorrowedBy")]
	public Member? Borrower { get; set; }
}