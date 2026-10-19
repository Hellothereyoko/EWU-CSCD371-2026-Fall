using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Tests;

[TestClass]
public class ContactBookTests
{
    // shared data and helpers to prevent accidental data mismatch from influencing test results
    private static readonly FullName TestName = new("John", "Smith");
    private const string TestEmail = "email";
    private static readonly ContactEmails TestContactEmails = new([TestEmail]);
    
    private static Student CreateStudent() => new(TestName, TestContactEmails);
    private static Employee CreateEmployee() => new(TestName, TestContactEmails);
    
    [TestMethod]
    public void ContactBook_AddStudent_AddsStudent()
    {
        ContactBook book = new();
    
        Student student = CreateStudent();
        book.Add(student);
        
        Assert.IsTrue(book.Contains(student));
    }
    
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
    
    [TestMethod]
    public void ContactBookWithStudent_AddingEqualDataEmployee_StoresBothContacts()
    {
        ContactBook book = new();
        
        Student student = CreateStudent();
        Employee employee = CreateEmployee();
        
        book.Add(student);
        book.Add(employee);
        
        Assert.IsTrue(book.Contains(student));
        Assert.IsTrue(book.Contains(employee));
        Assert.HasCount(2, book.GetAll());
    }
    
    [TestMethod]
    public void ContactBookWithStudent_ContainsWithEqualDataEmployee_ReturnsFalse()
    {
        ContactBook book = new();
        
        Student student = CreateStudent();
        Employee employee = CreateEmployee();
        
        book.Add(student);
        
        Assert.IsTrue(book.Contains(student));
        // collection behavior should not find an employee of equal data to a contained student as equal
        Assert.IsFalse(book.Contains(employee));
    }
}