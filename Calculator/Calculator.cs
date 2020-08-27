using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Calculator
    {
        public static double Calculate(string input)
        {
            input = input.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(input))
                return 0;

            //BODMAS
            Dictionary<string, Processor> operations =
                new Dictionary<string, Processor>();

            operations.Add("^", 
                new Processor(input, 
                "^", 
                "-?\\d+\\^-?\\d+",
                ProcessPowerRootDivisionAndMultipication,
                (x,y) => Math.Pow(x, y)));

            operations.Add("/",
                new Processor(input,
                "/",
                "-?\\d+\\/-?\\d+",
                ProcessPowerRootDivisionAndMultipication,
                (x, y) => { return x / y; }));

            operations.Add("*",
               new Processor(input,
               "*",
               "-?\\d+\\*-?\\d+",
               ProcessPowerRootDivisionAndMultipication,
               (x, y) => { return x * y; }));

            //operations.Add("+OR-", new Operation("[\\+\\-]?\\d+",
            //   (x, y) => { return x + y; }));

            string processedInput = string.Empty;
            foreach (var operation in operations)
            {
                processedInput = operation.Value.Process(operation.Value);
            }

            double result;
            if (double.TryParse(processedInput, out result))
                return result;

            return 0;
        }

        //pow
        public static string ProcessPowerRootDivisionAndMultipication(Processor operation)
        {
            Regex regex = new Regex(operation.Pattern);
            MatchCollection matches = regex.Matches(operation.Input);
            var input = operation.Input;

            foreach (Match match in matches)
            {
                double left = 0d;
                double right = 0d;
                double operationResult = 0d;

                var numbers = match.Value.Split(operation.Key);
                double.TryParse(numbers[0], out left);
                double.TryParse(numbers[1], out right);
                operationResult = operation.MathFunction(left, right);

                input = operation.Input.Replace(match.Value, operationResult.ToString());
            }

            return input;
        }
    }

    public class Processor
    {
        public readonly string Input;
        public readonly string Key;
        public readonly string Pattern;
        public Func<Processor, string> Process { get; }
        public Func<double, double, double> MathFunction { get; }

        public Processor(string input, 
            string key, 
            string pattern, 
            Func<Processor, string> process, 
            Func<double, double, double> mathProcessor)
        {
            Input = input;
            Key = key;
            Pattern = pattern;
            Process = process;
            MathFunction = mathProcessor;
        }
    }
}
