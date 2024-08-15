using Domain.Entities;

namespace Domain.Tests;

public class MemberTests
{
	[Fact]
	public void CanSetAndGetProperties()
	{
		var member = new Member
		{
			Id = 100,
			Name = "Ahmad",
			Email = "Ahmad@OP.com"
		};

		Assert.Equal(100, member.Id);
		Assert.Equal("Ahmad", member.Name);
		Assert.Equal("Ahmad@OP.com", member.Email);
	}

	[Fact]
	public void DefaultValues_ShouldBeAsExpected()
	{
		var member = new Member { Name = "undefined" };

		Assert.Equal(0, member.Id);
		Assert.Equal("undefined", member.Name);
		Assert.Null(member.Email);
	}
}