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
			List<Book>? books = libraryService.GetBorrowed();
			if(books is null)
				return Results.NotFound();
			
			return Results.Ok(books);
		});
		
		library.MapGet("/available", (LibraryService libraryService) =>
		{
			List<Book>? books = libraryService.GetAvailable();
			if(books is null)
				return Results.NotFound();
			
			return Results.Ok(books);
		});
		
		library.MapPost("/return", (LibraryService LibraryService, int bookId) =>
		{
			if (LibraryService.ReturnBook(bookId))
				return Results.Ok();

			return Results.StatusCode(500);
		});
		
		library.MapPost("/borrow", (LibraryService libraryService, int bookId, int memberId) =>
		{
			if (libraryService.BorrowBook(bookId, memberId))
				return Results.Ok();
			
			return Results.StatusCode(500);
		});
		
	}
}