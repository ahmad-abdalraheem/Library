using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Infrastructure.DataHandler;
using Domain.Entities;
using Xunit;

namespace Infrastructure.DataHandler.Tests
{
	public class DataHandlerTests
	{
		private readonly string _testFilePath = "test.json";

		private List<Book> CreateBookTestData()
		{
			return new List<Book>
			{
				new Book()
				{
					Id = 1,
					Title = "Test Book",
					Author = "Test Author",
					IsBorrowed = false
				}
			};
		}

		private List<Member> CreateMemberTestData()
		{
			return new List<Member>
			{
				new Member
				{
					Id = 1,
					Name = "Test Member"
				}
			};
		}

		[Fact]
		public void Write_ShouldWriteBooksToFile_WhenFileExists()
		{
			// Arrange
			var testData = CreateBookTestData();
			File.Create(_testFilePath).Close();
			var fileHandler = new DataHandler<Book>(_testFilePath);

			// Act
			var result = fileHandler.Write(testData);

			// Assert
			Assert.True(result);
			Assert.True(File.Exists(_testFilePath));

			var writtenData = File.ReadAllText(_testFilePath);
			Assert.Contains("\"Id\": 1", writtenData);

			// Cleanup
			File.Delete(_testFilePath);
		}

		[Fact]
		public void Write_ShouldWriteMembersToFile_WhenFileExists()
		{
			// Arrange
			var testData = CreateMemberTestData();
			File.Create(_testFilePath).Close();
			var fileHandler = new DataHandler<Member>(_testFilePath);

			// Act
			var result = fileHandler.Write(testData);

			// Assert
			Assert.True(result);
			Assert.True(File.Exists(_testFilePath));

			var writtenData = File.ReadAllText(_testFilePath);
			Assert.Contains("\"Id\": 1", writtenData);
			Assert.Contains("Test Member", writtenData);

			// Cleanup
			File.Delete(_testFilePath);
		}

		[Fact]
		public void Write_FileDoesNotExist_ThrowsFileNotFoundException_ReturnsFalse()
		{
			// Arrange
			var testData = CreateBookTestData();
			var fileHandler = new DataHandler<Book>("non_existent_file.json");

			// Act
			var result = fileHandler.Write(testData);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void Read_FileExists_ReadsFromFile_ReturnsList()
		{
			// Arrange
			var testData = CreateBookTestData();
			File.WriteAllText(_testFilePath, JsonSerializer.Serialize(testData));
			var fileHandler = new DataHandler<Book>(_testFilePath);

			// Act
			var result = fileHandler.Read();

			// Assert
			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(1, result[0].Id);

			// Cleanup
			File.Delete(_testFilePath);
		}

		[Fact]
		public void Read_FileDoesNotExist_ThrowsFileNotFoundException_ReturnsNull()
		{
			// Arrange
			var fileHandler = new DataHandler<Book>("non_existent_file.json");

			// Act
			var result = fileHandler.Read();

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void Write_ExceptionCaught_ReturnsFalse()
		{
			// Arrange
			var testData = CreateBookTestData();
			var fileHandler = new DataHandler<Book>("invalid_path\0.json"); // Invalid path to trigger exception

			// Act
			var result = fileHandler.Write(testData);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void Read_ExceptionCaught_ReturnsNull()
		{
			// Arrange
			var fileHandler = new DataHandler<Book>("invalid_path\0.json"); // Invalid path to trigger exception

			// Act
			var result = fileHandler.Read();

			// Assert
			Assert.Null(result);
		}
	}
}