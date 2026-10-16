using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Tests;

[TestClass]
public class ContactBookTests
{
    [TestMethod]
    public void ContactBook_AddStudent_AddsStudent()
    {
        ContactBook book = new();

        Student student = new(
            new FullName("John", "Smith"),
            "test@email");
        
        book.Add(student);
        
        Assert.IsTrue(book.Contains(student));
    }
    
    [TestMethod]
    public void Contains_EqualStudents_ReturnsTrue()
    {
        ContactBook book = new();
        
        Student stored = new(
            new FullName("John", "Smith"),
            "test@email");
        
        Student lookup = new(
            new FullName("John", "Smith"),
            "test@email");
        
        book.Add(stored);
        
        // validate stored and lookup are not same ref
        Assert.AreNotSame(stored, lookup);
        
        // equality wise the same however
        Assert.AreEqual(stored, lookup);
        
        // lookup by the not added ref should be true
        Assert.IsTrue(book.Contains(lookup));
    }
}