namespace Assignment8.PeopleImporter;

public sealed class Address
{
    public Address(string streetAddress, string city, string state, string zip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(streetAddress, nameof(streetAddress));
        ArgumentException.ThrowIfNullOrWhiteSpace(city, nameof(city));
        ArgumentException.ThrowIfNullOrWhiteSpace(state, nameof(state));
        ArgumentException.ThrowIfNullOrWhiteSpace(zip, nameof(zip));

        StreetAddress = streetAddress;
        City = city;
        State = state;
        Zip = zip;
    }

    public string StreetAddress { get; }

    public string City { get; }

    public string State { get; }

    public string Zip { get; }
}
