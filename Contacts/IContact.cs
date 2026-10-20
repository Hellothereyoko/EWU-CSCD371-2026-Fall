namespace Contacts;

public interface IContact
{
    ContactId Id { get; }
    FullName Name { get; }
    ContactEmails Emails { get; }
}

public record ContactId(char TypePrefix, int Number)
{
    public override string ToString() => $"{TypePrefix}:{Number}";
}

public record FullName(
    string FirstName,
    string LastName,
    string? MiddleName = null
);

public record ContactEmails(IReadOnlyList<string> Emails)
{
    public virtual bool Equals(ContactEmails? other)
    {
        // the same emails in any order should equate equal
        return other is not null
            && Emails.OrderBy(email => email)
                .SequenceEqual(
                    other.Emails.OrderBy(email => email)); 
    }
    
    public override int GetHashCode()
    {
        // note: the same emails in different order should return same hashcode
        HashCode hash = new();
        
        foreach (string email in Emails.OrderBy(email => email))
            hash.Add(email);
        
        return hash.ToHashCode();
    }
}

public abstract record BaseContact(FullName Name, ContactEmails Emails) : IContact
{
    public required ContactId Id { get; init; }

    // equals definition is shared between derivers
    public virtual bool Equals(BaseContact? other)
    {
        return other is not null
            && Name == other.Name
            && Emails == other.Emails;
    }
    
    // hashcode definition is shared between derivers
    public override int GetHashCode() => HashCode.Combine(Name, Emails);
}

public sealed record Student(FullName Name, ContactEmails Emails) : BaseContact(Name, Emails)
{
    // unique to Student
    public const char Prefix = 'S';
}

public sealed record Employee(FullName Name, ContactEmails Emails) : BaseContact(Name, Emails)
{
    // unique to Employee
    public const char Prefix = 'E';
}
