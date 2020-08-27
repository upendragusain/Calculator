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
            Dictionary<string, Operation> operations =
                new Dictionary<string, Operation>();

            operations.Add("^", new Operation("-?\\d+\\^-?\\d+",
                ProcessPowerAndRoot));

            //operations.Add("/", new Operation("-?\\d+\\/-?\\d+",
            //    (x, y) => { return x / y; }));

            //operations.Add("*", new Operation("-?\\d+\\*-?\\d+",
            //    (x, y) => { return x * y; }));

            //operations.Add("+OR-", new Operation("[\\+\\-]?\\d+",
            //   (x, y) => { return x + y; }));

            string processedInput = string.Empty;
            foreach (var operation in operations)
            {
                processedInput = operation.Value.Process(input, operation.Value.Pattern, operation.Key);
            }

            double result;
            if (double.TryParse(processedInput, out result))
                return result;

            return 0;
        }

        //pow
        public static string ProcessPowerAndRoot(string input, string pattern, string key)
        {
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                double left = 0d;
                double right = 0d;
                double operationResult = 0d;

                var numbers = match.Value.Split(key);
                double.TryParse(numbers[0], out left);
                double.TryParse(numbers[1], out right);
                operationResult = Math.Pow(left, right);

                input = input.Replace(match.Value, operationResult.ToString());
            }
            return input;
        }
    }

    public class Operation
    {
        public string Pattern { get; }
        public Func<string, string, string, string> Process { get; }
        public Operation(string pattern, Func<string, string, string, string> function)
        {
            Pattern = pattern;
            Process = function;
        }
    }
}
