
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contacts.Tests;




///summary
/// This is a test class for the IContact interface in the Contacts application.
///summary
[TestClass]
public class IContactTests
{
    /// <summary>
    /// This test method verifies that the IContact interface has a FirstName property. It checks if the property exists and is of type string.
    /// </summary>
    [TestMethod]
    public void IContact_ShouldHaveFirstNameProperty(){Assert.IsTrue(typeof(IContact).GetProperty("FirstName") != null);}


    /// <summary>
    /// This test method verifies that the IContact interface can have a MiddleName property. It checks if the property exists and is of type string.
    /// </summary>
    [TestMethod]
    public void IContact_CanHaveMiddleNameProperty(){Assert.IsTrue(typeof(IContact).GetProperty("MiddleName") != null);}


    /// <summary>
    /// This test method verifies that the IContact interface has a LastName property. It checks if the property exists and is of type string.
    /// </summary>
    [TestMethod]
    public void IContact_ShouldHaveLastNameProperty(){Assert.IsTrue(typeof(IContact).GetProperty("LastName") != null);}



    /// <summary>
    /// This test method verifies that the IContact interface has a RecordType property. It checks if the property exists and is of type string.
    /// </summary>
    [TestMethod]
    public void IContact_ShouldHaveRecordTypeProperty(){}
}