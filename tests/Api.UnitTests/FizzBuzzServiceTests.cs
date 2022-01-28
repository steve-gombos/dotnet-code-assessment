using FluentAssertions;
using Template.Api.Interfaces;
using Template.Api.Services;
using Xunit;

namespace Template.Api.UnitTests
{
    public class FizzBuzzServiceTests
    {
        private readonly IFizzBuzzService _sut;

        public FizzBuzzServiceTests()
        {
            _sut = new FizzBuzzService();
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(3, "Fizz")]
        [InlineData(4, "4")]
        [InlineData(5, "Buzz")]
        [InlineData(6, "Fizz")]
        [InlineData(7, "7")]
        [InlineData(8, "8")]
        [InlineData(9, "Fizz")]
        [InlineData(10, "Buzz")]
        [InlineData(11, "11")]
        [InlineData(12, "Fizz")]
        [InlineData(13, "13")]
        [InlineData(14, "14")]
        [InlineData(15, "FizzBuzz")]
        [InlineData(16, "16")]
        [InlineData(17, "17")]
        [InlineData(18, "Fizz")]
        [InlineData(19, "19")]
        [InlineData(20, "Buzz")]
        public void Test(int number, string expected)
        {
            // Act
            var actual = _sut.Generate(number);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
