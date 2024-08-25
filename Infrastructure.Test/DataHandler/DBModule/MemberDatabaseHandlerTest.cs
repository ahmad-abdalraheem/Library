using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Domain.Entities;
using Infrastructure.DataHandler;
using Moq;

public class MemberDatabaseHandlerTest
{
    private DbContextOptions<LibraryContext> GetInMemoryDatabaseOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    [Fact]
    public void Add_ShouldAddNewMember()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new MemberDbHandler<Member>(context);
            var members = new List<Member>
            {
                new Member() {Name = "John Doe"},
                new Member() { Name = "Jane Doe"}
            };

            // Act
            var result = handler.Add(members[0]) && handler.Add(members[1]);

            // Assert
            Assert.True(result);
            Assert.Equal(2, context.Members.Count());
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

        var handler = new MemberDbHandler<Member>(contextMock.Object);
        var members = new List<Member>
        {
            new Member() {Name = "John Doe"},
            new Member() { Name = "Jane Doe"}
        };

        // Act
        var result = handler.Add(members[0]);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Update_ShouldUpdateMember()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new MemberDbHandler<Member>(context);
            var members = new List<Member>
            {
                new Member() {Name = "John Doe"},
                new Member() { Name = "Jane Doe"}
            };

            // Act
            handler.Add(members[0]);
            handler.Add(null);
            members[0].Name = "New Name";
            var result = handler.Update(members[0]);

            // Assert
            Assert.True(result);
            Assert.Equal(1, context.Members.Count());
            Assert.Equal("New Name", context.Members.FirstOrDefault(b=> b.Name == "New Name").Name);
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
        var handler = new MemberDbHandler<Member>(contextMock.Object);
        
        // Act
        var result = handler.Update(null);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Delete_ShouldDeleteMember()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new MemberDbHandler<Member>(context);
            var book = new Member() { Name = "John Doe" };

            // Act
            handler.Add(book);
            var result = handler.Delete(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, context.Members.Count());
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
        var handler = new MemberDbHandler<Member>(contextMock.Object);
        
        // Act
        handler.Add(new Member() { Name = "John Doe" });
        var result = handler.Delete(15);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Get_ShouldReturnEntitiesFromDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new MemberDbHandler<Member>(context);
            var books = new List<Member>
            {
                new Member() {Name = "John Doe"},
                new Member() { Name = "Jane Doe"}
            };
            context.Members.AddRange(books);
            context.SaveChanges();

            // Act
            var result = handler.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == "John Doe");
            Assert.Contains(result, m => m.Name == "Jane Doe");
        }
    }
    
    [Fact]
    public void Get_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Member>>();
        var mockContext = new Mock<LibraryContext>(GetInMemoryDatabaseOptions(Guid.NewGuid().ToString()));

        // Set up the DbSet to throw an exception when enumerated
        mockSet.As<IQueryable<Member>>()
            .Setup(m => m.Provider)
            .Throws(new InvalidOperationException("Simulated database failure"));

        mockContext
            .Setup(c => c.Set<Member>())
            .Returns(mockSet.Object);

        var handler = new MemberDbHandler<Member>(mockContext.Object);

        // Act
        var result = handler.Get();

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetById_ShouldReturnMemberFromDatabase()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();
        var options = GetInMemoryDatabaseOptions(databaseName);
        using (var context = new LibraryContext(options))
        {
            var handler = new MemberDbHandler<Member>(context);
            var books = new List<Member>
            {
                new Member() {Name = "John Doe"},
                new Member() { Name = "Jane Doe"}
            };
            context.Members.AddRange(books);
            context.SaveChanges();

            // Act
            var result = handler.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }
    }
    
    [Fact]
    public void GetById_ShouldReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Member>>();
        var mockContext = new Mock<LibraryContext>(GetInMemoryDatabaseOptions(Guid.NewGuid().ToString()));

        // Set up the DbSet to throw an exception when enumerated
        mockSet.As<IQueryable<Member>>()
            .Setup(m => m.Provider)
            .Throws(new InvalidOperationException("Simulated database failure"));

        mockContext
            .Setup(c => c.Set<Member>())
            .Returns(mockSet.Object);

        var handler = new MemberDbHandler<Member>(mockContext.Object);

        // Act
        var result = handler.GetById(1);

        // Assert
        Assert.Null(result);
    }

}
