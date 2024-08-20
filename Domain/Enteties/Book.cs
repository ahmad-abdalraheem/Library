using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Book : IEntity
{
	public int Id { get; set; }
	public required string Title { get; set; }
	public required string Author { get; set; }
	public bool IsBorrowed { get; set; }
	public DateTime? BorrowedDate { get; set; }
	public int? BorrowedBy { get; set; }

	[NotMapped] public string? MemberName { get; set; }
}