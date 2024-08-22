using Application.Repository;
using Application.Service;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.DataHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		// Register DbContext
		services.AddDbContext<LibraryContext>(options =>
			options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=abdalraheem;Database=library;"));

		// Register Repositories with Scoped Lifetime
		services.AddScoped<IMemberRepository, MemberRepository>();
		services.AddScoped<IBookRepository, BookRepository>();

		// Register DataHandlers with Scoped Lifetime
		services.AddScoped<IDataHandler<Member>, DataDatabaseHandler<Member>>();
		services.AddScoped<IDataHandler<Book>, DataDatabaseHandler<Book>>();

		// Register Services with Scoped Lifetime
		services.AddScoped<MemberService>();
		services.AddScoped<BookService>();
		services.AddScoped<LibraryService>();

		return services;
	}
}