namespace Dag.Tests;


using Dag;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


/// <summary>
/// This class handles calc pgrm testing
/// </summary>
[TestClass]
public class calculateTests
{
    private Calculator? _calculator;

    [TestInitialize]
    public void TestInit()
    {
        _calculator = new();
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
    public void AddTests()
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
    public void SubTests()
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
    public void DivideByNumberTest()
    {
        const string testInput = "24 / 3";
        bool valid = _calculator!.TryCalculate(testInput, out int result);
        Assert.AreEqual(8, result);
        Assert.IsTrue(valid);
    }

    [TestMethod]
    public void DivideByZeroExceptionTest()
    {
        Assert.Throws<DivideByZeroException>(() => Calculator.Divide(0, 0));
    }


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