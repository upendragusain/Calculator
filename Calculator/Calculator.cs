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
                Math.Pow));

            operations.Add("/", new Operation("-?\\d+\\/-?\\d+",
                (x, y) => { return x / y; }));

            operations.Add("*", new Operation("-?\\d+\\*-?\\d+",
                (x, y) => { return x * y; }));

            operations.Add("+OR-", new Operation("[\\+\\-]?\\d+",
               (x, y) => { return x + y; }));

            foreach (var operation in operations)
            {
                Regex regex = new Regex(operation.Value.Pattern);
                MatchCollection matches = regex.Matches(input);

                if (operation.Key != "+OR-")
                {
                    foreach (Match match in matches)
                    {
                        double left = 0d;
                        double right = 0d;
                        double operationResult = 0;

                        var numbers = match.Value.Split(operation.Key);
                        double.TryParse(numbers[0], out left);
                        double.TryParse(numbers[1], out right);
                        operationResult = operation.Value.Function(left, right);

                        input = input.Replace(match.Value, operationResult.ToString());
                    }
                }
                else
                {
                    double value = 0d;
                    double operationResult = 0;
                    foreach (Match match in matches)
                    {
                        double.TryParse(match.Value, out value);
                        operationResult = operationResult + value;

                        input = input.Replace(match.Value, string.Empty);
                    }

                    input = operationResult.ToString();
                }
            }

            double result;
            if (double.TryParse(input, out result))
                return result;

            return 0;
        }
    }

    public class Operation
    {
        public string Pattern { get; }
        public Func<double, double, double> Function { get; }
        public Operation(string pattern, Func<double, double, double> function)
        {
            Pattern = pattern;
            Function = function;
        }
    }
}
