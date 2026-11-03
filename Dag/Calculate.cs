namespace Dag;

using System;
using System.Collections.Generic;

/// <summary>
/// This class is where all subclasses and methods for the program sit
/// </summary>
public class Program
{
    private readonly Calculator calculator;
    public Calculator CalculatorInstance => calculator; //Exposed for testing

    //Define two init-only setter properties, `WriteLine` and `ReadLine`, that contain delegates for writing a line of text and reading a line of text respectively
    public Action<string> WriteLine { get; init; } = Console.WriteLine;
    public Func<string?> ReadLine { get; init; } = Console.ReadLine;

    /// <summary>
    /// This is where the program runs it's contained classes
    /// </summary>
    public static void Main()
    {
        new Program();

    }// END Main method

    /// <summary>
    /// The actual program class where we implement things to be run in main
    /// </summary>
    public Program()
    {
        WriteLine("Program Initializing...");

        calculator = new Calculator();

        while (true)
        {
            WriteLine("Enter an expression to be evaluated: (example: 2 + 2)");
            string? inputExpression = ReadLine();

            if (calculator.TryCalc(inputExpression, out double result))
            {
                WriteLine($" = {result}");
            }
            else
            {
                WriteLine("Error processing input");
            }

            WriteLine("Enter 1 to evaluate another expression, or 2 to exit...");

            int response;
            while (!int.TryParse(ReadLine(), out response) || (response < 1 || response > 2))
            {
                WriteLine("Invalid input. Try again");
            }

            if (response == 2)
                break;
        }

    } //END Program Method



    //set the default behavior for the `WriteLine` and `ReadLine` properties to invoke `System.Console` versions of the methods and add an empty default constructor. ❌✔


    //- Define static `Add`, `Subtract`, `Multiply`, and `Divide` methods that have two parameters and return a third parameter. ✔
    //- Define a read-only property, `MathematicalOperations`, of type `System.Collections.Generics.IReadOnlyDictionary<TKey,TValue>` that:
    //- is initialized to a `System.Collections.Generics.Dictionary<<TKey,TValue>` instance that. ✔
    //    - Uses `char` for the key corresponding to the operators +, -, *, and /. ✔
    //  - Has values that correspond with the Add, Subtract, Multiply, and Divide methods. ✔
    //- Implement a `TryCalculate` method following "TryParse" pattern ✔
    //  - Valid `calculation` expressions include such strings as "3 + 4", "42 - 2", etc. ❌✔
    //- If there is no whitespace around the operator, you can assume the `calculation` is invalid and return false. Similarly if the operands are not integers. ❌✔
    //- Use `string.Split()`, pattern matching, logical and operators to parse the string in their entirety ❌✔
    //- Index into the `MathematicalOperations` method using the operator parsed during pattern matching to find the corresponding implementation and invoke it. ❌✔
    //- Implement the Program class to instantiate the calculator and invoke it based on user input from the console. ❌✔
    //- Be sure to use the `WriteLine`/`ReadLine` properties on `Program` for testing the input and output of your program. ❌✔

    /// <summary>
    /// Contains math methods used by the program as static methods passed as a ref
    /// </summary>
    public class Calculator
    {
        private IReadOnlyDictionary<char, Func<double, double, double>> MathematicalOperations { get; set; }

        //NOTES: Lvalue & Rvalue are the two params passed. Result is the return 
        private static double Add(double lValue, double rValue) => lValue + rValue;

        private static double Subtract(double lValue, double rValue) => lValue - rValue;

        private static double Multiply(double lValue, double rValue) => lValue * rValue;

        private static double Divide(double lValue, double rValue) => lValue / rValue;

        public Calculator()
        {
            MathematicalOperations = new Dictionary<char, Func<double, double, double>>
            {
                {'+', Add },
                {'-', Subtract },
                {'*', Multiply },
                {'/', Divide }
            };
        }

        /// <summary>
        /// This is responsible for executing a calc attempt using parse pattern
        /// NOTE: may be refactored into the actual pgrm loop
        /// </summary>
        public bool TryCalc(string? expr, out double result)
        {
            if (expr == null)
            {
                result = 0.0;
                return false;
            }

            var subStrings = expr.Split(' ');

            if (subStrings.Length == 3 &&
                double.TryParse(subStrings[0], out double argA) &&
                double.TryParse(subStrings[2], out double argB) &&
                MathematicalOperations.TryGetValue(subStrings[1].Trim()[0], out var doMath))
            {
                result = doMath(argA, argB);
                return true;
            }

            result = 0.0;
            return false;
        }

    } //END Calculator Class
} //END Global Program Class