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

	public override int SaveChanges()
	{
		UpdateIsBorrowedStatus();
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		UpdateIsBorrowedStatus();
		return await base.SaveChangesAsync(cancellationToken);
	}

	private void UpdateIsBorrowedStatus()
	{
		var entries = ChangeTracker.Entries<Book>()
			.Where(e => e is { State: EntityState.Modified, Entity.BorrowedBy: null });

		foreach (var entry in entries)
		{
			entry.Entity.IsBorrowed = false;
		}
	}
}