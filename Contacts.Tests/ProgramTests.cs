
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Contacts.Tests;
///summary
/// This is a test class for the Program class in the Contacts application.
/// /summary
[TestClass]
public class ProgramTests
{

  /// <summary>
  /// This test method verifies that the Main method of the Program class starts the application correctly. It captures the console output and checks if it contains the expected startup message.
  /// </summary>
  [TestMethod]
  public void Main_ShouldStartApplication()
  {
    // Arrange
    using var sw = new StringWriter();
    Console.SetOut(sw);

    // Act
    Program.Main();

    // Assert
    var expected = "Contacts Application Started!";
    var actual = sw.ToString().Trim();
    Assert.AreEqual(expected, actual);
  }

  // Student Tests  
    // Valid student data
    // Invalid student data


  // Employee Tests
    // Valid employee data
    // Invalid employee data  
}