using Application.Repository;
using Application.Service;
using Application.FileHandler;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.FileModule;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		var bookFilePath = "/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Books.json";
		var memberFilePath = "/home/ahmadabdalraheem/RiderProjects/Library/Infrastructure/Data/Members.json";

		services.AddSingleton<IMemberRepository, MemberRepository>();
		services.AddSingleton<IBookRepository, BookRepository>();

		services.AddSingleton<IFileHandler<Member>>(provider => new FileHandler<Member>(memberFilePath));
		services.AddSingleton<IFileHandler<Book>>(provider => new FileHandler<Book>(bookFilePath));

		services.AddTransient<MemberService>();
		services.AddTransient<BookService>();
		services.AddTransient<LibraryService>();

		return services;
	}
}