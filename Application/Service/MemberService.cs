using AutoMapper;
using Domain.Entities;
using Domain.Repository;

namespace Application.Service;

public sealed class MemberService(IMemberRepository memberRepository, IMapper mapper)
{
	private readonly IMemberRepository _memberRepository =
		memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
	public GetMemberDto Add(AddMemberDto member)
	{
		return mapper.Map<GetMemberDto>(_memberRepository.Add(mapper.Map<Member>(member)));
	}

	public GetMemberDto Update(UpdateMemberDto member)
	{
		return mapper.Map<GetMemberDto>(_memberRepository.Update(mapper.Map<Member>(member)));
	}

	public void Delete(int memberId)
	{
		_memberRepository.Delete(memberId);
	}

	public List<GetMemberDto>? Get() => mapper.Map<List<GetMemberDto>>(_memberRepository.Get());

	public GetMemberDto? GetById(int memberId) => mapper.Map<GetMemberDto>(_memberRepository.GetById(memberId));
}