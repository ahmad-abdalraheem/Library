using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfile;

public class MemberProfile : Profile
{
	public MemberProfile()
	{
		CreateMap<AddMemberDto, Member>().ReverseMap();
	}
}