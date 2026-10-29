namespace Dag.Tests;


using Dag;
using Microsoft.VisualStudio.TestTools.UnitTesting;


/// <summary>
/// This class handles calc pgrm testing
/// </summary>
[TestClass]
public class calculateTests
{

    /// <summary>
    /// This test launches the program & checks if Calculator obj is instantiated
    /// </summary>
    [TestMethod]
    public void CalculatorObjInitTest(){
        
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

        //Act

        //Assert
    }


    /// <summary>
    /// Tests subtractions operations
    /// </summary>
    [TestMethod]
    public void subTests()
    {
        
        //Arrange

        //Act

        //Assert
    }


    /// <summary>
    /// Tests multiplication operations
    /// </summary>
    [TestMethod]
    public void multTests()
    {
        
        //Arrange

        //Act

        //Assert
    }


    /// <summary>
    /// Tests division operations
    /// </summary>
    [TestMethod]
    public void divTests()
    {
        
        //Arrange

        //Act

        //Assert
    }
}