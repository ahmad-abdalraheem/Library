using Application.FileHandler;
using Application.Repository;
using Application.Service;
using Domain.Entities;
using Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Tests
{
	public class ServiceCollectionExtensionsTests
	{
		[Fact]
		public void AddApplicationServices_RegistersServicesCorrectly()
		{
			var services = new ServiceCollection();

			services.AddApplicationServices();
			var serviceProvider = services.BuildServiceProvider();

			var memberRepository = serviceProvider.GetService<IMemberRepository>();
			Assert.NotNull(memberRepository);
			Assert.IsType<MemberRepository>(memberRepository);

			var memberHandler = serviceProvider.GetService<IFileHandler<Member>>();
			Assert.NotNull(memberHandler);

			var memberService = serviceProvider.GetService<MemberService>();
			Assert.NotNull(memberService);

			var bookRepository = serviceProvider.GetService<IBookRepository>();
			Assert.NotNull(bookRepository);
			Assert.IsType<BookRepository>(bookRepository);

			var bookHandler = serviceProvider.GetService<IFileHandler<Book>>();
			Assert.NotNull(bookHandler);

			var bookService = serviceProvider.GetService<BookService>();
			Assert.NotNull(bookService);

			var libraryService = serviceProvider.GetService<LibraryService>();
			Assert.NotNull(libraryService);
		}
	}
}