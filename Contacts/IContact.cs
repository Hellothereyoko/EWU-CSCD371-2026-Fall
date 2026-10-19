namespace Contacts;

public interface IContact
{
    Guid Id { get; }
    FullName Name { get; }
    ContactEmails Emails { get; }
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

public record Student(FullName Name, ContactEmails Emails) : IContact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    // NOTE: equality is only defined between Student Instances
    // any comparison against non-Student type is false
    public virtual bool Equals(Student? other)
    {
        // intentionally leaving ID property out of equality check
        return other is not null
               && Name == other.Name
               && Emails == other.Emails;
    }

    // again, intentionally leaving ID property out of hash calc
    // allows two Student records where only ID's differ to have same Hash
    public override int GetHashCode() => HashCode.Combine(Name, Emails);
}

public record Employee(FullName Name, ContactEmails Emails) : IContact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public virtual bool Equals(Employee? other)
    {
        return other is not null
               && Name == other.Name
               && Emails == other.Emails;
    }
    
    public override int GetHashCode() => HashCode.Combine(Name, Emails);
}
