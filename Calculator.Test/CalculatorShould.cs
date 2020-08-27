using System;
using Xunit;

namespace Calculator.Test
{
    public class CalculatorShould
    {
        [Theory]
        [InlineData("    3*5+123+11^3-45+5*2^12+39/13", 21907)]
        public void DoMultipleOperations1(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("    1*2*3*4*5", 120)]
        // ToDo: devision needs to be done one at a  time (order matters here)
        [InlineData("    100/10/5/2/1", 1)]
        public void DoSameOperationMultipleTimes(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }


        [Theory]
        [InlineData("    1*2*3*4*5", 120)]
        [InlineData("    1*2*3*4/4*5+27-89", -32)]
        public void DoMultipleOperations2(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("    2^5  ", 32)]
        [InlineData("    -2^5  ", -32)]
        public void DoPowers(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }


        [Theory]
        [InlineData("    2^-5  ", 0.03125)]
        [InlineData("    -2^-5  ", -0.03125)]
        public void DoRoots(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("    12/5  ", 2.4)]
        [InlineData("    -39/3  ", -13)]
        [InlineData("     39/-3  ", -13)]
        [InlineData("    -39/-3  ", 13)]
        [InlineData("    39/3  ", 13)]
        //https://stackoverflow.com/questions/58764510/visual-studio-2019-error-pop-up-appears-to-prevent-an-unsafe-abort-when-evalu
        //[InlineData("    1/3  ", 0.3333333333333333)]
        public void DoDivision(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("     3*5  ", 15)]
        [InlineData("    -3*5  ", -15)]
        [InlineData("     3*-5  ", -15)]
        [InlineData("    -3*-5  ", 15)]
        public void DoMultiplication(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("     5+3  ", 8)]
        [InlineData("     5-3  ", 2)]
        [InlineData("    -5+3  ", -2)]
        [InlineData("    -5-3  ", -8)]
        [InlineData("    +5+3  ", 8)]
        [InlineData("    +5-3  ", 2)]

        public void DoAdditionOrSubtract(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("     1.67-0.78  ", 0.89)]
        [InlineData("     15-5.99  ", 9.01)]
        [InlineData("    -15 + 3.78  ", -11.22)]
        [InlineData("    -15.001 + 0.003  ", -14.998)]
        [InlineData("    -15 - 3  ", -18)]

        public void DoAdditionOrSubtractWithDecimal(string input, decimal output)
        {
            var result = Calculator.Calculate(input);
            Assert.Equal(output, result);
        }
    }
}
