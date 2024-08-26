using Application.Repository;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.DataHandler;
using Moq;

namespace Application.Tests;

public class MemberRepositoryTests
{
	private readonly MemberRepository _memberRepository;
	private readonly Mock<IDataHandler<Member>> _mockMemberHandler;

	public MemberRepositoryTests()
	{
		_mockMemberHandler = new Mock<IDataHandler<Member>>();
		_memberRepository = new MemberRepository(_mockMemberHandler.Object);
	}

	[Fact]
	public void Add_ShouldAddMember()
	{
		var initialMembers = new List<Member>
		{
			new() { Id = 1, Name = "Existing Member", Email = "existing@example.com" }
		};
		var newMember = new Member { Name = "New Member", Email = "new@example.com" };
		_mockMemberHandler.Setup(handler => handler.Get()).Returns(initialMembers);
		_mockMemberHandler.Setup(handler => handler.Add(It.IsAny<Member>())).Returns(true);

		var result = _memberRepository.Add(newMember);

		Assert.True(result);
		Assert.Equal(2, initialMembers.Count);
		Assert.Equal("New Member", initialMembers[1].Name);
		_mockMemberHandler.Verify(handler => handler.Add(It.IsAny<Member>()), Times.Once);
	}

	[Fact]
	public void Update_ShouldUpdateMember()
	{
		var initialMembers = new List<Member>
		{
			new() { Id = 1, Name = "Old Name", Email = "old@example.com" }
		};
		var updatedMember = new Member { Id = 1, Name = "Updated Name", Email = "updated@example.com" };
		_mockMemberHandler.Setup(handler => handler.Get()).Returns(initialMembers);
		_mockMemberHandler.Setup(handler => handler.Update(It.IsAny<Member>())).Returns(true);

		var result = _memberRepository.Update(updatedMember);

		Assert.True(result);
		Assert.Equal("Updated Name", initialMembers[0].Name);
		_mockMemberHandler.Verify(handler => handler.Update(It.IsAny<Member>()), Times.Once);
	}

	[Fact]
	public void Delete_ShouldRemoveMember()
	{
		var initialMembers = new List<Member>
		{
			new() { Id = 1, Name = "Member to Remove", Email = "remove@example.com" },
			new() { Id = 2, Name = "Another Member", Email = "another@example.com" }
		};
		_mockMemberHandler.Setup(handler => handler.Get()).Returns(initialMembers);
		_mockMemberHandler.Setup(handler => handler.Delete(It.IsAny<int>())).Returns(true);

		var result = _memberRepository.Delete(1);

		Assert.True(result);
		Assert.Single(initialMembers);
		Assert.Equal(2, initialMembers[0].Id);
		_mockMemberHandler.Verify(handler => handler.Delete(It.IsAny<int>()), Times.Once);
	}

	[Fact]
	public void Get_ShouldReturnMembers()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "Member 1", Email = "member1@example.com" },
			new() { Id = 2, Name = "Member 2", Email = "member2@example.com" }
		};
		_mockMemberHandler.Setup(handler => handler.Get()).Returns(members);

		var result = _memberRepository.Get();

		Assert.NotNull(result);
		Assert.Equal(2, result.Count);
		Assert.Equal("Member 1", result[0].Name);
		Assert.Equal("Member 2", result[1].Name);
	}

	[Fact]
	public void GetById_ShouldReturnCorrectMember()
	{
		var members = new List<Member>
		{
			new() { Id = 1, Name = "Member 1", Email = "member1@example.com" },
			new() { Id = 2, Name = "Member 2", Email = "member2@example.com" }
		};
		_mockMemberHandler.Setup(handler => handler.GetById(2)).Returns(members[1]);
	
		var result = _memberRepository.GetById(2);
	
		Assert.NotNull(result);
		Assert.Equal(2, result.Id);
		Assert.Equal("Member 2", result.Name);
	}

	[Fact]
	public void Add_ShouldThrowFileLoadExceptionWhenMembersAreNull()
	{
		_mockMemberHandler.Setup(handler => handler.Get()).Returns((List<Member>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _memberRepository.Add(new Member
		{
			Name = "undefined"
		}));
	}

	[Fact]
	public void Update_ShouldThrowFileLoadExceptionWhenMembersAreNull()
	{
		_mockMemberHandler.Setup(handler => handler.Get()).Returns((List<Member>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _memberRepository.Update(new Member
		{
			Name = "undefined"
		}));
	}

	[Fact]
	public void Delete_ShouldThrowFileLoadExceptionWhenMembersAreNull()
	{
		_mockMemberHandler.Setup(handler => handler.Get()).Returns((List<Member>)null);

		Assert.Throws<FailWhileLoadingDataException>(() => _memberRepository.Delete(1));
	}
}