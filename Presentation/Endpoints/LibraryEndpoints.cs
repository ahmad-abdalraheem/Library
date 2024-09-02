using Application.Service;
using Domain.Entities;

namespace Presentation.Endpoints;

public static class LibraryEndpoints
{
	public static void RegisterLibraryEndpoints(this IEndpointRouteBuilder routes)
	{
		var library = routes.MapGroup("/api/v1/library");

		library.MapGet("/borrowed", (LibraryService libraryService) =>
		{
			try
			{
				List<GetBookDto> books = libraryService.GetBorrowed();

				return Results.Ok(books);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Library > Borrowed : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		library.MapGet("/available", (LibraryService libraryService) =>
		{
			try
			{
				List<GetBookDto> books = libraryService.GetAvailable();

				return Results.Ok(books);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Library > Available : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		library.MapPost("/return", (LibraryService libraryService, int bookId) =>
		{
			try
			{
				if (bookId <= 0)
					throw new IndexOutOfRangeException("Book Id cannot be negative or zero.");

				GetBookDto book = libraryService.ReturnBook(bookId);
				return Results.Ok(book);
			}
			catch (Exception e) when (e is KeyNotFoundException or IndexOutOfRangeException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Return : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		library.MapPost("/borrow", (LibraryService libraryService, int bookId, int memberId) =>
		{
			try
			{
				if (bookId <= 0)
					throw new IndexOutOfRangeException("Book Id cannot be negative or zero.");
				if (memberId <= 0)
					throw new IndexOutOfRangeException("Member Id cannot be negative or zero.");

				GetBookDto book = libraryService.BorrowBook(bookId, memberId);
				return Results.Ok(book);
			}
			catch (Exception e) when (e is KeyNotFoundException or IndexOutOfRangeException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Borrow : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});
	}
}