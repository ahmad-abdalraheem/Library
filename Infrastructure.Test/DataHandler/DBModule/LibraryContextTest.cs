using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Domain.Entities;

public class LibraryContextTests
{
    private DbContextOptions<LibraryContext> GetInMemoryDatabaseOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public async Task SaveChanges_ShouldUpdateIsBorrowedStatus_WhenBookIsModified()
    {
        // Arrange
        var databaseName = "SaveChanges_ShouldUpdateIsBorrowedStatus_WhenBookIsModified";
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                IsBorrowed = true
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Act
            book.BorrowedBy = null;
            context.Books.Update(book);
            await context.SaveChangesAsync();
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
