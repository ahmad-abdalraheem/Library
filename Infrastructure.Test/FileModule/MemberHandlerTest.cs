using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.FileModule;

namespace Infrastructure.Tests;

public class MemberHandlerTests
{
	private readonly MemberHandler _memberHandler;
	private readonly string _testFilePath = "MembersTest.json";

	public MemberHandlerTests()
	{
		_memberHandler = new MemberHandler();
		_memberHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(_memberHandler, _testFilePath);
	}

	[Fact]
	public void Write_ShouldWriteDataToFile()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "Member 1", Email = "member1@example.com" }
		};

		var result = _memberHandler.Write(members);

		Assert.True(result);
		Assert.True(File.Exists(_testFilePath));

		var json = File.ReadAllText(_testFilePath);
		var deserializedMembers = JsonSerializer.Deserialize<List<Member>>(json);
		Debug.Assert(deserializedMembers != null, nameof(deserializedMembers) + " != null");
		Assert.Single(deserializedMembers);
		Assert.Equal("Member 1", deserializedMembers[0].Name);
	}

	[Fact]
	public void Read_ShouldReadDataFromFile()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "Member 1", Email = "member1@example.com" }
		};
		File.WriteAllText(_testFilePath, JsonSerializer.Serialize(members));

		var result = _memberHandler.Read();

		Assert.NotNull(result);
		Assert.Single(result);
		Assert.Equal("Member 1", result[0].Name);
	}

	[Fact]
	public void Write_ShouldHandleException()
	{
		var invalidPathHandler = new MemberHandler();
		invalidPathHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(invalidPathHandler, "InvalidPath/Members.json");

		var result = invalidPathHandler.Write(new List<Member>());

		Assert.False(result);
	}

	[Fact]
	public void Read_ShouldHandleException()
	{
		var invalidPathHandler = new MemberHandler();
		invalidPathHandler.GetType().GetField("_filePath", BindingFlags.NonPublic | BindingFlags.Instance)
			?.SetValue(invalidPathHandler, "InvalidPath/Members.json");

		var result = invalidPathHandler.Read();

		Assert.Null(result);
	}
}