using Domain.Entities;

namespace Domain.Tests;

public class BookTests
{
	[Fact]
	public void CanSetAndGetProperties()
	{
		var book = new Book()
		{
			Id = 100,
			Title = "Book Title",
			Author = "Book Author",
			IsBorrowed = true,
			BorrowedDate = DateTime.Now,
			BorrowedBy = 123,
			MemberName = "John Doe"
		};

		Assert.Equal(100, book.Id);
		Assert.Equal("Book Title", book.Title);
		Assert.Equal("Book Author", book.Author);
		Assert.True(book.IsBorrowed);
		Assert.NotNull(book.BorrowedDate);
		Assert.Equal(123, book.BorrowedBy);
		Assert.Equal("John Doe", book.MemberName);
	}

	[Fact]
	public void DefaultValues_ShouldBeAsExpected()
	{
		var book = new Book()
		{
			Title = "undefined",
			Author = "undefined"
		};

		Assert.Equal(0, book.Id);
		Assert.Equal("undefined", book.Title);
		Assert.Equal("undefined", book.Author);
		Assert.False(book.IsBorrowed);
		Assert.Null(book.BorrowedDate);
		Assert.Null(book.BorrowedBy);
		Assert.Null(book.MemberName);
	}
}