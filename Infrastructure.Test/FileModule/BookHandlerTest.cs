using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.FileModule;

namespace Infrastructure.Tests;

public class BookHandlerTests
{
	private readonly BookHandler _bookHandler;
	private readonly string _testFilePath = "BooksTest.json";

	public BookHandlerTests()
	{
		_bookHandler = new BookHandler();
		_bookHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(_bookHandler, _testFilePath);
	}

	[Fact]
	public void Write_ShouldWriteDataToFile()
	{
		var books = new List<Book>
		{
			new() { Id = 1, Title = "Book 1", Author = "Author 1" }
		};

		var result = _bookHandler.Write(books);

		Assert.True(result);
		Assert.True(File.Exists(_testFilePath));

		var json = File.ReadAllText(_testFilePath);
		var deserializedBooks = JsonSerializer.Deserialize<List<Book>>(json);
		Assert.Single(deserializedBooks);
		Assert.Equal("Book 1", deserializedBooks[0].Title);
	}

	[Fact]
	public void Read_ShouldReadDataFromFile()
	{
		var books = new List<Book>
		{
			new() { Id = 1, Title = "Book 1", Author = "Author 1" }
		};
		File.WriteAllText(_testFilePath, JsonSerializer.Serialize(books));

		var result = _bookHandler.Read();

		Assert.NotNull(result);
		Assert.Single(result);
		Assert.Equal("Book 1", result[0].Title);
	}

	[Fact]
	public void Write_ShouldHandleException()
	{
		var invalidPathHandler = new BookHandler();
		invalidPathHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(invalidPathHandler, "InvalidPath/Books.json");

		var result = invalidPathHandler.Write(new List<Book>());

		Assert.False(result);
	}

	[Fact]
	public void Read_ShouldHandleException()
	{
		var invalidPathHandler = new BookHandler();
		invalidPathHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(invalidPathHandler, "InvalidPath/Books.json");

		var result = invalidPathHandler.Read();

		Assert.Null(result);
	}

	public void Dispose()
	{
		if (File.Exists(_testFilePath)) File.Delete(_testFilePath);
	}
}