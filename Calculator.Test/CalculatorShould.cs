using System;
using Xunit;

namespace Calculator.Test
{
    public class CalculatorShould
    {
        //[Fact]
        //public void DoPowers()
        //{
        //    //var result = Calculator.Calculate(" 2 + 2 ");
        //    //var result = Calculator.Calculate("    3*5+123+11^3-45+5*2^12+39/13");

        //    var result = Calculator.Calculate("    2^5  ");

        //    result = Calculator.Calculate("    -2^5  ");

        //}

        [Theory]
        [InlineData("    2^5  ", 32d)]
        [InlineData("    -2^5  ", -32d)]
        public void DoPowers(string input, double output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }


        [Theory]
        [InlineData("    2^-5  ", 0.03125d)]
        [InlineData("    -2^-5  ", -0.03125d)]
        public void DoRoots(string input, double output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("    12/5  ", 2.4d)]
        [InlineData("    -39/3  ", -13d)]
        [InlineData("     39/-3  ", -13d)]
        [InlineData("    -39/-3  ", 13d)]
        [InlineData("    39/3  ", 13d)]
        [InlineData("    1/3  ", 0.3333333333333333d)]
        public void DoDivision(string input, double output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("     3*5  ", 15d)]
        [InlineData("    -3*5  ", -15d)]
        [InlineData("     3*-5  ", -15d)]
        [InlineData("    -3*-5  ", 15d)]
        public void DoMultiplication(string input, double output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("     5+3  ", 8d)]
        [InlineData("     5-3  ", 2d)]
        [InlineData("    -5+3  ", -2d)]
        [InlineData("    -5-3  ", -8d)]
        [InlineData("    +5+3  ", 8d)]
        [InlineData("    +5-3  ", 2d)]

        public void DoAdditionOrSubtract(string input, double output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }
    }
}
