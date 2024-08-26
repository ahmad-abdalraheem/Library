using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Infrastructure.DataHandler;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class BookDbHandlerTests
{
    private DbContextOptions<LibraryContext> GetInMemoryDatabaseOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public void Add_ShouldAddNewBook()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new BookDbHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };

            // Act
            var result = handler.Add(books[0]) && handler.Add(books[1]);

            // Assert
            Assert.True(result);
            Assert.Equal(2, context.Books.Count());
        }
    }

    [Fact]
    public void Add_ShouldReturnFalse_WhenExceptionOccurs()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var contextMock = new Mock<LibraryContext>(options);
        contextMock
            .Setup(ctx => ctx.SaveChanges())
            .Throws(new InvalidOperationException("Database does not exist"));

        var handler = new BookDbHandler<Book>(contextMock.Object);
        var books = new List<Book>
        {
            new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
            new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
        };

        // Act
        var result = handler.Add(books[0]);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Update_ShouldUpdateBook()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new BookDbHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };

            // Act
            handler.Add(books[0]);
            handler.Add(null);
            books[0].Author = "New Author";
            var result = handler.Update(books[0]);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Books.Count());
            Assert.Equal("New Author", context.Books.FirstOrDefault(b=> b.Title == "Book 1").Author);
        }
    }

    [Fact]
    public void Update_ShouldReturnFalse_WhenExceptionOccurs()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var contextMock = new Mock<LibraryContext>(options);
        var handler = new BookDbHandler<Book>(contextMock.Object);
        
        // Act
        var result = handler.Update(null);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Delete_ShouldDeleteBook()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new BookDbHandler<Book>(context);
            var book = new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true };

            // Act
            handler.Add(book);
            var result = handler.Delete(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, context.Books.Count());
        }
    }

    [Fact]
    public void Delete_ShouldReturnFalse_WhenExceptionOccurs()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var contextMock = new Mock<LibraryContext>(options);
        var handler = new BookDbHandler<Book>(contextMock.Object);
        
        // Act
        handler.Add(new Book() {Id = 1, Title = "Book 1", Author = "Author 1", IsBorrowed = false });
        var result = handler.Delete(15);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Get_ShouldReturnBooksFromDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new BookDbHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Act
            var result = handler.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, b => b.Title == "Book 1");
            Assert.Contains(result, b => b.Title == "Book 2");
        }
    }
    
    [Fact]
    public void Get_ShouldReturnNull_WhenExceptionIsThrown()
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

        var handler = new BookDbHandler<Book>(mockContext.Object);

        // Act
        var result = handler.Get();

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetById_ShouldReturnBookFromDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new BookDbHandler<Book>(context);
            var books = new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = false },
                new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Act
            var result = handler.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Book 1", result.Title);
        }
    }
    
    [Fact]
    public void GetById_ShouldReturnNull_WhenExceptionIsThrown()
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

        var handler = new BookDbHandler<Book>(mockContext.Object);

        // Act
        var result = handler.GetById(1);

        // Assert
        Assert.Null(result);
    }

}
