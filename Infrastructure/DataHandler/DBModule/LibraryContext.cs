using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
	public DbSet<Book> Books { get; set; }
	public DbSet<Member> Members { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Book>()
			.HasOne(b => b.Borrower)
			.WithMany()
			.HasForeignKey(b => b.BorrowedBy)
			.OnDelete(DeleteBehavior.SetNull);
	}

	public void UpdateIsBorrowedStatus(Member borrower)
	{
		List<Book> borrowedBooks = Books.Where(b => b.Borrower == borrower).ToList();
		foreach (var book in borrowedBooks)
		{
			book.IsBorrowed = false;
			book.BorrowedDate = null;
		}
	}
}