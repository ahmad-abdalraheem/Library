using Application.Repository;
using Application.Service;
using Domain.FileHandler;
using Domain.Repository;
using Infrastructure.FileModule;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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

            var memberHandler = serviceProvider.GetService<IMemberHandler>();
            Assert.NotNull(memberHandler);
            Assert.IsType<MemberHandler>(memberHandler);

            var memberService = serviceProvider.GetService<MemberService>();
            Assert.NotNull(memberService);

            var bookRepository = serviceProvider.GetService<IBookRepository>();
            Assert.NotNull(bookRepository);
            Assert.IsType<BookRepository>(bookRepository);

            var bookHandler = serviceProvider.GetService<IBookHandler>();
            Assert.NotNull(bookHandler);
            Assert.IsType<BookHandler>(bookHandler);

            var bookService = serviceProvider.GetService<BookService>();
            Assert.NotNull(bookService);

            var libraryService = serviceProvider.GetService<LibraryService>();
            Assert.NotNull(libraryService);
        }
    }
}
