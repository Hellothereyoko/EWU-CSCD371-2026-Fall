using Contacts;

namespace Persistence;

public interface IDataService
{
    void Save(IEnumerable<IContact> contacts);

    IContact[] Load();
}