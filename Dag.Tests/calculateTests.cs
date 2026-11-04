namespace Dag.Tests;


using Dag;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


/// <summary>
/// This class handles calc pgrm testing
/// </summary>
[TestClass]
public class CalculateTests
{
    private Calculator? _calculator;

    [TestInitialize]
    public void TestInit()
    {
        _calculator = new();
    }


    /// <summary>
    /// This test checks if the Program class throws ArgumentNullException when null delegates are passed to its constructor. It verifies that the correct parameter names are included in the exception messages.
    /// </summary>
    [TestMethod]
    public void ProgramNullDelegatesThrowArgumentNullException()
    {
        var writeLineException = Assert.Throws<ArgumentNullException>(
            () => new Program(null!, Console.ReadLine));
        Assert.AreEqual("writeLine", writeLineException.ParamName);

        var readLineException = Assert.Throws<ArgumentNullException>(
            () => new Program(Console.WriteLine, null!));
        Assert.AreEqual("readLine", readLineException.ParamName);
    }

    [TestMethod]
    public void ProgramNullInputExitsGracefully()
    {
        var output = new List<string>();
        var program = new Program(output.Add, () => null);

        program.Run();

        Assert.AreSequenceEqual(
            ["Enter a calculation in the form: number operator number", "Expression: ", "Goodbye."], output);
    }


    /// <summary>
    /// This test checks if the Program class correctly writes the result of a calculation to the output when a valid expression is provided. It simulates user input and captures the output for verification.
    /// </summary>
    [TestMethod]
    public void ProgramWritesCalcToScreen()
    {
        var output = new List<string>();
        var input = new Queue<string?>(new string?[] { "2 + 2", null });
        var program = new Program(output.Add, input.Dequeue);

        program.Run();

        CollectionAssert.AreEqual(
            new[]
            {
                "Enter a calculation in the form: number operator number",
                "Expression: ",
                "Result: 4",
                "Expression: ",
                "Goodbye."
            },
            output);
    }

    /// <summary>
    /// This test launches the program & checks if Calculator obj is instantiated
    /// </summary>
    [TestMethod]
    public void CalculatorObjInitTest()
    {
        var program = new Program();
        Assert.IsNotNull(program.CalculatorInstance);
    }


    /// <summary>
    /// Tests addition operations performed
    /// </summary>
    [TestMethod]
    public void AddTest()
    {
        //Arrange
        const string testInput = "4 + 5";
        //Act
        bool valid = _calculator!.TryCalculate(testInput, out int result);

        //Assert
        Assert.AreEqual(9, result);
        Assert.IsTrue(valid);
    }


    /// <summary>
    /// Tests subtractions operations
    /// </summary>
    [TestMethod]
    public void SubTest()
    {

        //Arrange
        const string testInput = "9 - 4";
        //Act
        bool valid = _calculator!.TryCalculate(testInput, out int result);
        //Assert
        Assert.AreEqual(5, result);
        Assert.IsTrue(valid);
    }


    /// <summary>
    /// Tests multiplication operations
    /// </summary>
    [TestMethod]
    public void MultTests()
    {
        //Arrange
        const string testInput = "9 * 4";
        //Act
        bool valid = _calculator!.TryCalculate(testInput, out int result);
        //Assert
        Assert.AreEqual(36, result);
        Assert.IsTrue(valid);
    }


    /// <summary>
    /// Tests division operations
    /// </summary>
    [TestMethod]
    public void DivideTest()
    {
        const string testInput = "24 / 3";
        bool valid = _calculator!.TryCalculate(testInput, out int result);
        Assert.AreEqual(8, result);
        Assert.IsTrue(valid);
    }

    /// <summary>
    /// Tests division by zero exception
    /// </summary>
    [TestMethod]
    public void DivideByZeroExceptionTest()
    {
        Assert.Throws<DivideByZeroException>(() => Calculator.Divide(0, 0));
    }


    /// <summary>
    /// Tests invalid expressions and checks if the calculator returns false for them
    /// </summary>
    /// <param name="testInput"></param>
    [TestMethod]
    [DataRow("1 hello 3")]
    [DataRow("1 2")]
    [DataRow("1 / 0")]
    [DataRow(" ")]
    [DataRow("1 + four")]
    [DataRow("1 % 2")]
    public void InvalidExpressionTest(string testInput)
    {
        bool valid = _calculator!.TryCalculate(testInput!, out int _);
        Assert.IsFalse(valid);
    }

}
