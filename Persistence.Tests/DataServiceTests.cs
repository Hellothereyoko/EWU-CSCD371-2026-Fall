using Contacts;

namespace Persistence.Tests;

[TestClass]
public class DataServiceTests
{
    private static readonly FullName TestName = new("John", "Smith");
    private static readonly ContactEmails TestContactEmails = new(["test@email"]);
    
    private static Student CreateStudent() => new(TestName, TestContactEmails);
    private static Employee CreateEmployee() => new(TestName, TestContactEmails);
    
    [TestMethod]
    public void SaveThenLoad_ReturnsSavedContacts()
    {
        // arrange
        FakeDataService service = new();

        IContact[] createdList =
        [
            CreateStudent(),
            CreateEmployee()
        ];
        
        // act
        service.Save(createdList);
        IContact[] savedList = service.Load();
        
        // assert
        CollectionAssert.AreEqual(createdList, savedList);
    }
    
    [TestMethod]
    public void Save_WithExistingList_ReplacesExistingContacts()
    {
        // arrange
        FakeDataService service = new();
        
        IContact[] originalList =
        [
            CreateStudent()
        ];
        
        service.Save(originalList);
        
        // act
        IContact[] overwriteList =
        [
            CreateStudent(),
            CreateEmployee()
        ];
        
        service.Save(overwriteList);
        IContact[] savedList = service.Load();
        
        // assert
        CollectionAssert.AreNotEqual(originalList, savedList);
        CollectionAssert.AreEqual(overwriteList, savedList);
    }
}

public class FakeDataService : IDataService
{
    private IContact[] _contacts = [];

    public void Save(IEnumerable<IContact> contacts)
    {
        _contacts = contacts.ToArray();
    }
    
    public IContact[] Load() => _contacts;
}