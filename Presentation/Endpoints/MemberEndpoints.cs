using Application.Service;
using Domain.Entities;

namespace Presentation.Endpoints;

public static class MemberEndpoints
{
	public static void RegisterMemberEndpoints(this IEndpointRouteBuilder routes)
	{
		var members = routes.MapGroup("/api/v1/members");

		members.MapGet("/", (MemberService memberService) =>
		{
			try
			{
				List<Member>? membersList = memberService.Get();

				return Results.Ok(membersList);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError\x1b[0m> Members >  Get > " + e.Message);
				return Results.Problem(statusCode: 500, detail: e.Message);
			}
		});

		members.MapGet("/{id}", (MemberService membersService, int id) =>
		{
			try
			{
				Member? member = membersService.GetById(id);
				if (member is null)
					return Results.NotFound();

				return Results.Ok(member);
			}
			catch (Exception e) when (e is IndexOutOfRangeException or KeyNotFoundException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"\x1b[41mError\x1b[0m Members > GetById > {e.Message}");
				return Results.Problem(statusCode: 500, detail: e.Message);
			}
		});

		members.MapPost("/", (MemberService memberService, Member member) =>
		{
			try
			{
				member.Id = 0;
				Member result = memberService.Add(member);

				return Results.Created($"/api/v1/members/{result.Id}", result);
			}
			catch (ArgumentException argumentException)
			{
				return Results.BadRequest(argumentException.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"\x1b[41mError\x1b[0m Members > Add > {e.Message}");
				return Results.Problem(statusCode: 500, detail: e.Message);
			}
		});

		members.MapPut("/", (MemberService memberService, Member member) =>
		{
			try
			{
				Member result = memberService.Update(member);

				return Results.Ok(result);
			}
			catch (ArgumentException argumentException)
			{
				return Results.BadRequest(argumentException.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"\x1b[41mError\x1b[0m> Members > Update > {e.Message}");
				return Results.Problem(statusCode: 500, detail: e.Message);
			}
		});

		members.MapDelete("/{id}", (MemberService memberService, int id) =>
		{
			try
			{
				if (id <= 0)
					throw new IndexOutOfRangeException("Member Id cannot be negative or zero.");

				memberService.Delete(id);
				return Results.NoContent();
			}
			catch (Exception e) when (e is IndexOutOfRangeException or KeyNotFoundException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"\x1b[41mError\x1b[0m> Members > Delete > {e.Message}");
				return Results.Problem(statusCode: 500, detail: e.Message);
			}
		});
	}
}