using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Domain.Entities;
using Infrastructure.DataHandler;
using Moq;

public class DataDatabaseHandlerTests
{
    private DbContextOptions<LibraryContext> GetInMemoryDatabaseOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public void Write_ShouldAddEntitiesToDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new DataDatabaseHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };

            // Act
            var result = handler.Write(books);

            // Assert
            Assert.True(result);
            Assert.Equal(2, context.Books.Count());
        }
    }

    [Fact]
    public void Write_ShouldReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var contextMock = new Mock<LibraryContext>(options);
        contextMock
            .Setup(ctx => ctx.SaveChanges())
            .Throws(new InvalidOperationException("Database does not exist"));

        var handler = new DataDatabaseHandler<Book>(contextMock.Object);
        var books = new List<Book>
        {
            new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
            new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
        };

        // Act
        var result = handler.Write(books);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Read_ShouldReturnEntitiesFromDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new DataDatabaseHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Act
            var result = handler.Read();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, b => b.Title == "Book 1");
            Assert.Contains(result, b => b.Title == "Book 2");
        }
    }
    
    [Fact]
    public void Read_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Book>>();
        var mockContext = new Mock<LibraryContext>(GetInMemoryDatabaseOptions(Guid.NewGuid().ToString()));

        // Set up the DbSet to throw an exception when enumerated
        mockSet.As<IQueryable<Book>>()
            .Setup(m => m.Provider)
            .Throws(new InvalidOperationException("Simulated database failure"));

        mockContext
            .Setup(c => c.Set<Book>())
            .Returns(mockSet.Object);

        var handler = new DataDatabaseHandler<Book>(mockContext.Object);

        // Act
        var result = handler.Read();

        // Assert
        Assert.Null(result);
    }

}
