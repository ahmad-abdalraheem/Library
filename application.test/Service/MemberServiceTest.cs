using Application.Service;
using Domain.Entities;
using Domain.Repository;
using Moq;

namespace Application.Tests;

public class MemberServiceTests
{
	private readonly MemberService _memberService;
	private readonly Mock<IMemberRepository> _mockMemberRepository;

	public MemberServiceTests()
	{
		_mockMemberRepository = new Mock<IMemberRepository>();
		_memberService = new MemberService(_mockMemberRepository.Object);
	}

	[Fact]
	public void Add_ShouldReturnTrue_WhenAddSucceeds()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Add(member)).Returns(true);

		var result = _memberService.Add(member);

		Assert.True(result);
	}

	[Fact]
	public void Add_ShouldReturnFalse_WhenAddFails()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Add(member)).Returns(false);

		var result = _memberService.Add(member);

		Assert.False(result);
	}

	[Fact]
	public void Add_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Add(member)).Throws(new Exception("Database error"));

		var result = _memberService.Add(member);

		Assert.False(result);
	}

	[Fact]
	public void Update_ShouldReturnTrue_WhenUpdateSucceeds()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Update(member)).Returns(true);

		var result = _memberService.Update(member);

		Assert.True(result);
	}

	[Fact]
	public void Update_ShouldReturnFalse_WhenUpdateFails()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Update(member)).Returns(false);

		var result = _memberService.Update(member);

		Assert.False(result);
	}

	[Fact]
	public void Update_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		var member = new Member { Id = 1, Name = "John Doe" };
		_mockMemberRepository.Setup(repo => repo.Update(member)).Throws(new Exception("Database error"));

		var result = _memberService.Update(member);

		Assert.False(result);
	}

	[Fact]
	public void Delete_ShouldReturnTrue_WhenDeleteSucceeds()
	{
		_mockMemberRepository.Setup(repo => repo.Delete(1)).Returns(true);

		var result = _memberService.Delete(1);

		Assert.True(result);
	}

	[Fact]
	public void Delete_ShouldReturnFalse_WhenDeleteFails()
	{
		_mockMemberRepository.Setup(repo => repo.Delete(1)).Returns(false);

		var result = _memberService.Delete(1);

		Assert.False(result);
	}

	[Fact]
	public void Delete_ShouldReturnFalse_WhenExceptionIsThrown()
	{
		_mockMemberRepository.Setup(repo => repo.Delete(1)).Throws(new Exception("Database error"));

		var result = _memberService.Delete(1);

		Assert.False(result);
	}

	[Fact]
	public void Get_ShouldReturnMembers_WhenGetSucceeds()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "John Doe" },
			new() { Id = 2, Name = "Jane Smith" }
		};
		_mockMemberRepository.Setup(repo => repo.Get()).Returns(members);

		var result = _memberService.Get();

		Assert.NotNull(result);
		Assert.Equal(2, result.Count);
	}

	[Fact]
	public void Get_ShouldReturnNull_WhenGetFails()
	{
		_mockMemberRepository.Setup(repo => repo.Get()).Returns((List<Member>)null);

		var result = _memberService.Get();

		Assert.Null(result);
	}

	[Fact]
	public void Get_ShouldReturnNull_WhenExceptionIsThrown()
	{
		_mockMemberRepository.Setup(repo => repo.Get());

		var result = _memberService.Get();

		Assert.Null(result);
	}

	[Fact]
	public void GetById_ShouldReturnMember_WhenMemberIsExist()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "John Doe" },
			new() { Id = 2, Name = "Jane Smith" }
		};
		_mockMemberRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<int>(id => members.FirstOrDefault(m => m.Id == id));

		// Act
		var result = _memberService.GetById(2);

		Assert.NotNull(result);
		Assert.Equal(2, result.Id);
		Assert.Equal("Jane Smith", result.Name);
	}
	
	[Fact]
	public void GetById_ShouldReturnNull_WhenMemberNotExist()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "John Doe" },
			new() { Id = 2, Name = "Jane Smith" }
		};
		_mockMemberRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns<int>(id => members.FirstOrDefault(m => m.Id == id));

		// Act
		var result = _memberService.GetById(3);

		Assert.Null(result);
	}
}