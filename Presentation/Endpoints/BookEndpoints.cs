using System.ComponentModel.DataAnnotations;
using Application.Service;
using Domain.Entities;
using FluentValidation;
using Presentation.Validation;
using ValidationException = FluentValidation.ValidationException;

namespace Presentation.Endpoints;

public static class BooksEndpoints
{
	public static void RegisterBookEndpoints(this IEndpointRouteBuilder routes)
	{
		var books = routes.MapGroup("/api/v1/books");

		books.MapGet("", (BookService bookService) =>
		{
			try
			{
				List<GetBookDto>? booksList = bookService.Get();
				return Results.Ok(booksList);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Get : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		books.MapGet("/{id}", (BookService booksService, int id) =>
		{
			try
			{
				GetBookDto? book = booksService.GetById(id);
				if (book == null)
					throw new KeyNotFoundException("No book with Id : " + id);

				return Results.Ok(book);
			}
			catch (Exception e) when (e is IndexOutOfRangeException or KeyNotFoundException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > GetById : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		books.MapPost("/", (BookService bookService, AddBookDto book) =>
		{
			try
			{
				new AddBookValidator().ValidateAndThrow(book);

				GetBookDto result = bookService.Add(book);
				return Results.Created($"/api/v1/books/{result.Id}", result);
			}
			catch (ValidationException validationException)
			{
				return Results.BadRequest(validationException.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Add : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		books.MapPut("/", (BookService bookService, UpdateBookDto book) =>
		{
			try
			{
				new UpdateBookValidator().ValidateAndThrow(book);
				GetBookDto result = bookService.Update(book);

				return Results.Ok(result);
			}
			catch (ValidationException validationException)
			{
				return Results.BadRequest(validationException.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Update : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});

		books.MapDelete("/{id}", (BookService bookService, int id) =>
		{
			try
			{
				bookService.Delete(id);
				return Results.NoContent();
			}
			catch (Exception e) when (e is IndexOutOfRangeException or KeyNotFoundException)
			{
				return Results.NotFound(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("\x1b[41mError \x1b[0m Books > Delete : " + e.Message);
				return Results.Problem(detail: e.Message, statusCode: 500);
			}
		});
	}
}