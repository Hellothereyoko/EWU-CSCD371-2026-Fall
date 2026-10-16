namespace Contacts;

public record Student(FullName Name, string Email) : IContact
{
    // Get Init structure means at creation if object initializer not used, a new Guid is provided
    // More importantly, a saved ID for this record can be object initialized when loading records to a book
    public Guid Id { get; init; } = Guid.NewGuid();
    
    // NOTE: equality is only defined between Student Instances
    // any comparison against non-Student type is false
    // This means comparison between otherwise identical Student and Employee is not equal
    public virtual bool Equals(Student? other)
    {
        // intentionally leaving ID property out of equality check
        // Allows Students where only ID's differ to be considered equal
        return other is not null 
               && Name == other.Name 
               && Email == other.Email;
    }
    
    // again, intentionally leaving ID property out of hash calc
    // allows two Student records where only ID's differ to have same Hash
    public override int GetHashCode() => HashCode.Combine(Name, Email);
}