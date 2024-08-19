using System;
using System.IO;
using Xunit;

namespace ConsoleApp.Tests
{
    public class UserConsoleTests
    {
        private readonly StringWriter _stringWriter;
        private readonly UserConsole _userConsole;

        public UserConsoleTests()
        {
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
            Console.SetError(_stringWriter);
            _userConsole = new UserConsole();
        }

        [Fact]
        public void Write_WritesToConsole()
        {
            // Act
            _userConsole.Write("Hello, World!");

            // Assert
            var output = _stringWriter.ToString();
            Assert.Contains("Hello, World!", output);
        }
        
        [Fact]
        public void WriteLine_WritesLineToConsole()
        {
            _userConsole.WriteLine("Hello, World!");
            _userConsole.WriteLine();
            
            var output = _stringWriter.ToString();
            Assert.Contains("Hello, World!", output);
            Assert.EndsWith(Environment.NewLine, output);
        }

        [Fact]
        public void Clear_ClearsConsole()
        {
            _userConsole.Clear();

            var output = _stringWriter.ToString();
            Assert.Equal(string.Empty, output);
        }

        [Fact]
        public void ReadLine_ReadsInputFromConsole()
        {
            var input = "Test input";
            Console.SetIn(new StringReader(input));

            var result = _userConsole.ReadLine();

            Assert.Equal(input, result);
        }

        [Fact]
        public void AddKeySequence_ThrowsException()
        {
            Assert.Throws<NotImplementedException>(
                () => _userConsole.AddKeySequence([]));
        }

    }
}
