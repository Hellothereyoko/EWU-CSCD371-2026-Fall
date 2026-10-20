namespace Contacts;


/// <summary>
/// This class represents a contact book that can store and manage contacts implementing the IContact interface.
/// </summary>
public class ContactBook
{
    /// <summary>
    /// This property holds a collection of contacts in the contact book. It is implemented as a HashSet to ensure that each contact is unique and to provide efficient operations for adding, removing, and checking for the existence of contacts.
    /// </summary>
    private HashSet<IContact> Entities { get; } = [];


    /// <summary>
    /// This method adds a new contact to the contact book. If the contact already exists in the collection, it will not be added again, ensuring that each contact is unique.
    /// </summary>
    /// <param name="item"></param>
    public void Add(IContact item)
    {
        Entities.Add(item);

        string contactType = item.GetType().Name;
        int itemIdNumber = item.Id.Number;
        
        // if we have no value for this prefix, make one.
        // or the value for this prefix is lower than this items id + 1
        if (!_nextIds.TryGetValue(contactType, out int currentNext) || currentNext <= itemIdNumber)
            _nextIds[contactType] = itemIdNumber + 1;
    }


    /// <summary>
    /// This method removes a contact from the contact book. If the contact does not exist in the collection, no action is taken.
    /// </summary>
    /// <param name="item"></param>
    public void Remove(IContact item)
    {
        Entities.Remove(item);
    }


    /// <summary>
    /// This method checks if a specific contact exists in the contact book. It returns true if the contact is found, and false otherwise.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(IContact item)
    {
        return Entities.Contains(item);
    }


    /// <summary>
    /// This method retrieves all contacts currently stored in the contact book. It returns an array of IContact objects representing the contacts.
    /// </summary>
    /// <returns></returns>
    public IContact[] GetAll() => Entities.ToArray();
    
    private readonly Dictionary<string, int> _nextIds = [];

    public int GetNextId(string contactType)
    {
        // returns value or if prefix not exist, default 1
        // NOTE: GetNextID does not mutate the dictionary, only actually adding the IContact item does
        // this just allows program to see what the next value for a prefix should be
        return _nextIds.GetValueOrDefault(contactType, 1);
    }
}




/// Notes:
/// --------------------------------------------------
/// 
/// 
/// - `ContactBook` must keep storing contacts in a `HashSet<IContact>` (a provided test checks this). ❌✔
/// - Adding a contact equal to a stored one must not add a duplicate, and the caller must be able to tell. ❌✔
/// - `Contains` and `Remove` must work with an equal but different instance. ❌✔
/// - Add whatever else your CLI needs (for example, search or list). ❌✔
///
/// 
/// The `HashSet` is what makes equality matter: it calls `Equals` and `GetHashCode` through `IContact`, so a wrong implementation shows up as duplicate or "missing" contacts.