using Application.Service;
using Domain.Entities;

namespace API.Endpoints;

public static class MemberEndpoints
{
	public static void RegisterMemberEndpoints(this IEndpointRouteBuilder routes)
	{
		var members = routes.MapGroup("/api/v1/members");

		members.MapGet("", (MemberService memberService) =>
		{
			List<Member>? membersList = memberService.Get();
			if(membersList is null)
				return Results.NotFound();
			
			return Results.Ok(membersList);

		});

		members.MapGet("/{id}", (MemberService membersService, int id) =>
		{
			Member? member = membersService.GetById(id);
			if(member is null)
				return Results.NotFound();

			return Results.Ok(member);

		});

		members.MapPost("/add", (MemberService memberService, Member member) =>
		{
			bool result = memberService.Add(member);
			if(!result)
				return Results.StatusCode(500);
			
			return Results.Created();
		});

		members.MapPost("update", (MemberService memberService, Member member) =>
		{
			bool result = memberService.Update(member);
			if(!result)
				return Results.StatusCode(500);

			return Results.Ok();
		});

		members.MapDelete("/delete", (MemberService memberService, int memberId) =>
		{
			bool result = memberService.Delete(memberId);
			if(!result)
				return Results.StatusCode(500);
			
			return Results.Ok();
		});
	}
}