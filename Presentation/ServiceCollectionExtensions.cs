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
		services.AddDbContext<LibraryContext>(options =>
			options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=abdalraheem;Database=library;"));

		// Register Repositories
		services.AddScoped<IMemberRepository, MemberRepository>();
		services.AddScoped<IBookRepository, BookRepository>();

		// Register Services
		services.AddScoped<MemberService>();
		services.AddScoped<BookService>();
		services.AddScoped<LibraryService>();

		return services;
	}
	
	public static IHost CreateHost()
	{
		return Host.CreateDefaultBuilder()
			.ConfigureServices(services => services.AddApplicationServices())
			.Build();
	}
}

