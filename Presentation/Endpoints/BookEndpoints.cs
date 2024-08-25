using Application.Service;
using Domain.Entities;

namespace Presentation.Endpoints;

public static class BooksEndpoints
{
	public static void RegisterBookEndpoints(this IEndpointRouteBuilder routes)
	{
		var books = routes.MapGroup("/api/v1/books");

		books.MapGet("", (BookService bookService) =>
		{
			List<Book>? booksList = bookService.Get();
			if(booksList is null)
				return Results.NotFound();
			
			return Results.Ok(booksList);

		});

		books.MapGet("/{id}", (BookService booksService, int id) =>
		{
			Book? book = booksService.GetById(id);
			if(book is null)
				return Results.NotFound();

			return Results.Ok(book);

		});

		books.MapPost("/", (BookService bookService, Book book) =>
		{
			bool result = bookService.Add(book);
			if(!result)
				return Results.StatusCode(500);
			
			return Results.Created();
		});

		books.MapPut("/{id}", (BookService bookService, int id, Book book) =>
		{
			bool result = bookService.Update(book);
			if(!result)
				return Results.StatusCode(500);

			return Results.Ok();
		});

		books.MapDelete("/{id}", (BookService bookService, int id) =>
		{
			bool result = bookService.Delete(id);
			if(!result)
				return Results.StatusCode(500);
			
			return Results.Ok();
		});
	}
}