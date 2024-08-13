using Application.Repository;
using Application.Service;
using Domain.FileHandler;
using Domain.Repository;
using Infrastructure.FileModule;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddSingleton<IMemberRepository, MemberRepository>();
		services.AddSingleton<IMemberHandler, MemberHandler>();
		services.AddTransient<MemberService>();

		services.AddSingleton<IBookRepository, BookRepository>();
		services.AddSingleton<IBookHandler, BookHandler>();
		services.AddTransient<BookService>();

		services.AddTransient<LibraryService>();
		return services;
	}
}