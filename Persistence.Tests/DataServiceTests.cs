using Contacts;

namespace Persistence.Tests;

[TestClass]
public sealed class DataServiceTests
{
    [TestMethod]
    public void SaveThenLoad_ReturnsSavedContacts()
    {
        // arrange
        FakeDataService service = new();

        IContact[] createdList =
        [
            new Student(
                new FullName("John", "Smith"),
                "test@email"),

            new Employee(
                new FullName("John", "Smith"),
                "test@email")
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
            new Student(
                new FullName("John", "Smith"),
                "test@email"),
        ];
        
        service.Save(originalList);
        
        // act
        IContact[] overwriteList =
        [
            new Student(
                new FullName("New", "Name"),
                "test@email"),
            
            new Employee(
                new FullName("John", "Smith"),
                "test@email")
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