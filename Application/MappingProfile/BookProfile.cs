using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfile;

public class BookProfile : Profile
{
	public BookProfile()
	{
		CreateMap<Book, GetBookDto>().ReverseMap();

		CreateMap<AddBookDto, Book>()
			.ForMember(dest => dest.BorrowedDate, 
				opt => opt.MapFrom(src => (DateOnly?)null)) //
			.ReverseMap();

		CreateMap<UpdateBookDto, Book>().ReverseMap();

		CreateMap<GetBookDto, UpdateBookDto>().ReverseMap();
		
		CreateMap<AddBookDto, GetBookDto>().ReverseMap();
	}
}