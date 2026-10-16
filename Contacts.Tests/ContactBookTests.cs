using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Tests;

///summary
/// This is a test class for the ContactBook class in the Contacts application.
///summary
[TestClass]
public class ContactBookTests
{
    // shared data and helpers to prevent accidental data mismatch from influencing test results
    private static readonly FullName TestName = new("John", "Smith");
    private const string TestEmail = "email";
    
    private static Student CreateStudent() => new(TestName, TestEmail);
    private static Employee CreateEmployee() => new(TestName, TestEmail);
    
    [TestMethod]
    public void ContactBook_AddStudent_AddsStudent()
    {
        ContactBook book = new();

        Student student = CreateStudent();
        
        book.Add(student);
        
        Assert.IsTrue(book.Contains(student));
    }
    
    // Validates that building a new Contact with identical data as a stored Contact returns a true from Contains
    [TestMethod]
    public void Contains_EqualStudents_ReturnsTrue()
    {
        ContactBook book = new();

        Student stored = CreateStudent();
        
        Student lookup = CreateStudent();
        
        book.Add(stored);
        
        // validate stored and lookup are not same ref
        Assert.AreNotSame(stored, lookup);
        
        // equality wise the same however
        Assert.AreEqual(stored, lookup);
        
        // lookup by the not added ref should be true
        Assert.IsTrue(book.Contains(lookup));
    }
}