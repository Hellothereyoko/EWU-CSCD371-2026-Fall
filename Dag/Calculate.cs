namespace Dag;

using System;
using System.ComponentModel;
using Microsoft.VisualBasic;

public class Program
{
    
//Define two init-only setter properties, `WriteLine` and `ReadLine`, that contain delegates for writing a line of text and reading a line of text respectively
	public Action<string> WriteLine { get; init; } = Console.WriteLine;
	public Func<string?> ReadLine { get; init; } = Console.ReadLine;

	
//Instantiate the Calculator class



//



}



//set the default behavior for the `WriteLine` and `ReadLine` properties to invoke `System.Console` versions of the methods and add an empty default constructor. ❌✔


//- Define static `Add`, `Subtract`, `Multiply`, and `Divide` methods that have two parameters and return a third parameter. ❌✔
//- Define a read-only property, `MathematicalOperations`, of type `System.Collections.Generics.IReadOnlyDictionary<TKey,TValue>` that:
//- is initialized to a `System.Collections.Generics.Dictionary<<TKey,TValue>` instance that. ❌✔
  //    - Uses `char` for the key corresponding to the operators +, -, *, and /. ❌✔
    //  - Has values that correspond with the Add, Subtract, Multiply, and Divide methods. ❌✔
//- Implement a `TryCalculate` method following "TryParse" pattern ❌✔
  //  - Valid `calculation` expressions include such strings as "3 + 4", "42 - 2", etc. ❌✔
    //- If there is no whitespace around the operator, you can assume the `calculation` is invalid and return false. Similarly if the operands are not integers. ❌✔
    //- Use `string.Split()`, pattern matching, logical and operators to parse the string in their entirety ❌✔
    //- Index into the `MathematicalOperations` method using the operator parsed during pattern matching to find the corresponding implementation and invoke it. ❌✔
//- Implement the Program class to instantiate the calculator and invoke it based on user input from the console. ❌✔
//- Be sure to use the `WriteLine`/`ReadLine` properties on `Program` for testing the input and output of your program. ❌✔
public class Calculator
{
    
    private static void Add(){} //Has two params and returns a 3rd

    private static void Subtract(){} //Has two params and returns a 3rd

    private static void Multiply(){} //Has two params and returns a 3rd

    private static void Divide(){} //Has two params and returns a 3rd


    /// <summary>
    /// This is responsible for executing a calc attempt using parse pattern
    /// </summary>
    private void TryCalculate(){}

}