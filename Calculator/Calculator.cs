using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    // ToDo: decimal in the input f's it up
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

            operations.Add("+-",
               new Processor(input,
               "+-",
               "\\+?\\-?\\d+",
               ProcessAdditionOrSubtration,
               null));

            string processedInput = operations.First().Value.Input;
            foreach (var operation in operations)
            {
                operation.Value.Input = processedInput;
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

            foreach (Match match in matches)
            {
                double left = 0d;
                double right = 0d;
                double operationResult = 0d;

                var numbers = match.Value.Split(operation.Key);
                double.TryParse(numbers[0], out left);
                double.TryParse(numbers[1], out right);
                operationResult = operation.MathFunction(left, right);

                operation.Input = operation.Input.Replace(match.Value, operationResult.ToString());
            }

            return operation.Input;
        }

        public static string ProcessAdditionOrSubtration(Processor operation)
        {
            if (!operation.Input.Contains("+") && !operation.Input.Contains("-"))
                return operation.Input;

            if (operation.Input.Contains("."))
                return operation.Input;

            Regex regex = new Regex(operation.Pattern);
            MatchCollection matches = regex.Matches(operation.Input);

            double value = 0d;
            double operationResult = 0;
            foreach (Match match in matches)
            {
                double.TryParse(match.Value, out value);
                operationResult = operationResult + value;
            }

            return matches.Count > 0 ? operationResult.ToString() : operation.Input;
        }
    }

    public class Processor
    {
        public string Input;
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
