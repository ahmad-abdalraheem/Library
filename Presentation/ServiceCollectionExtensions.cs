using Application.Repository;
using Application.Service;
using Infrastructure.DataHandler;
using Domain.Entities;
using Domain.Repository;
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

		services.AddSingleton<IDataHandler<Member>>(provider => new DataFileHandler<Member>(memberFilePath));
		services.AddSingleton<IDataHandler<Book>>(provider => new DataFileHandler<Book>(bookFilePath));

		services.AddSingleton<MemberService>();
		services.AddSingleton<BookService>();
		services.AddSingleton<LibraryService>();

		return services;
	}
}