namespace Domain.Entities;

public class GetBookDto
{
	public int Id { get; set; }
	
	public required string Title { get; set; }
	
	public required string Author { get; set; }
	
	public bool IsBorrowed { get; set; }
	
	public DateOnly BorrowedDate { get; set; }
	
	public Member? Borrower { get; set; }
}