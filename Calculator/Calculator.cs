using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Calculator
{
    //https://regex101.com/
    public class Calculator
    {
        //28 decimal places (decimal precision)
        private const string NUMBER_WITH_28_PRECISION 
            = @"\d+\.?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?\d?";

        public static decimal Calculate(string input)
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
                $@"{NUMBER_WITH_28_PRECISION}\^\-?{NUMBER_WITH_28_PRECISION}",
                ProcessPowerRootDivisionAndMultipication,
                (x, y) => (decimal)Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))));

            operations.Add("/",
                new Processor(input,
                "/",
                $@"{NUMBER_WITH_28_PRECISION}\/\-?{NUMBER_WITH_28_PRECISION}",
                ProcessPowerRootDivisionAndMultipication,
                (x, y) => { return x / y; }));

            operations.Add("*",
               new Processor(input,
               "*",
               $@"{NUMBER_WITH_28_PRECISION}\*\-?{NUMBER_WITH_28_PRECISION}",
               ProcessPowerRootDivisionAndMultipication,
               (x, y) => { return x * y; }));

            operations.Add("+-",
               new Processor(input,
               "+-",
               $@"\+?\-?{NUMBER_WITH_28_PRECISION}", 
               ProcessAdditionOrSubtration,
               null));

            string processedInput = operations.First().Value.Input;
            foreach (var operation in operations)
            {
                operation.Value.Input = processedInput;
                processedInput = operation.Value.Process(operation.Value);
            }

            decimal result;
            if (decimal.TryParse(processedInput, out result))
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
                decimal left = 0;
                decimal right = 0;
                decimal operationResult = 0;

                var numbers = match.Value.Split(operation.Key);
                decimal.TryParse(numbers[0], out left);
                decimal.TryParse(numbers[1], out right);
                operationResult = operation.MathFunction(left, right);

                var operationResultString = operationResult.ToString();

                operation.Input = operation.Input
                    .Replace(match.Value, operationResultString)
                    .Replace("--", string.Empty);
            }

            return operation.Input;
        }

        public static string ProcessAdditionOrSubtration(Processor operation)
        {
            Regex regex = new Regex(operation.Pattern);
            MatchCollection matches = regex.Matches(operation.Input);

            decimal value = 0m;
            decimal operationResult = 0m;
            foreach (Match match in matches)
            {
                decimal.TryParse(match.Value, out value);
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
        public Func<decimal, decimal, decimal> MathFunction { get; }

        public Processor(string input, 
            string key, 
            string pattern, 
            Func<Processor, string> process, 
            Func<decimal, decimal, decimal> mathProcessor)
        {
            Input = input;
            Key = key;
            Pattern = pattern;
            Process = process;
            MathFunction = mathProcessor;
        }
    }
}
