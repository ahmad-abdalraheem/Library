using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class LibraryContextTests
{
    private DbContextOptions<LibraryContext> GetInMemoryDatabaseOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public async Task UpdateIsBorrowedStatus_ShouldUpdateIsBorrowedStatus_WhenBookIsModified()
    {
        // Arrange
        var databaseName = "SaveChanges_ShouldUpdateIsBorrowedStatus_WhenBookIsModified";
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            Member borrower = new Member()
            {
                Id = 1,
                Name = "Borrower",
                Email = null
            };
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                IsBorrowed = true,
                Borrower = borrower
            };
            context.Books.Add(book);
            context.SaveChanges();
            
            // Act
            context.UpdateIsBorrowedStatus(borrower);
            context.SaveChanges();
        }

        // Assert
        using (var context = new LibraryContext(GetInMemoryDatabaseOptions(databaseName)))
        {
            var updatedBook = context.Books.First();
            Assert.False(updatedBook.IsBorrowed);
        }
    }

    [Fact]
    public async Task SaveChanges_ShouldNotModifyIsBorrowedStatus_WhenBorrowedByIsNotNull()
    {
        // Arrange
        var databaseName = "SaveChanges_ShouldNotModifyIsBorrowedStatus_WhenBorrowedByIsNotNull";
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var member = new Member { Name = "Test Member" };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                BorrowedBy = member.Id,
                IsBorrowed = true
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Act
            book.Title = "Updated Title";
            context.Books.Update(book);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new LibraryContext(GetInMemoryDatabaseOptions(databaseName)))
        {
            var updatedBook = context.Books.First();
            Assert.True(updatedBook.IsBorrowed);
        }
    }
}
