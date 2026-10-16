using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Tests;

[TestClass]
public class ContactTests
{
    [TestMethod]
    public void Students_WithSameData_HaveDifferentIds()
    {
        Student student1 = new(
            new FullName("John", "Smith"),
            "test@email");
        
        Student student2 = new(
            new FullName("John", "Smith"),
            "test@email");
        
        Assert.AreNotEqual(student1.Id, student2.Id);
    }

    [TestMethod]
    public void Students_WithSameData_AreEqual()
    {
        Student student1 = new(
            new FullName("John", "Smith"),
            "test@email");
        
        Student student2 = new(
            new FullName("John", "Smith"),
            "test@email");
        
        Assert.AreEqual(student1, student2);
    }
    
    [TestMethod]
    public void Students_WithSameData_HaveSameHashCode()
    {
        Student student1 = new(
            new FullName("John", "Smith"),
            "test@email.com");

        Student student2 = new(
            new FullName("John", "Smith"),
            "test@email.com");

        Assert.AreEqual(
            student1.GetHashCode(),
            student2.GetHashCode());
    }
    
    [TestMethod]
    public void Student_And_Employee_WithSameData_AreNotEqual()
    {
        IContact student = new Student(
            new FullName("John", "Smith"),
            "test@email.com");

        IContact employee = new Employee(
            new FullName("John", "Smith"),
            "test@email.com");

        Assert.AreNotEqual(student, employee);
    }
}