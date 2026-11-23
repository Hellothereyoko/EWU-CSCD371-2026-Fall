namespace Assignment.Interfaces;

/// <summary>Represents one person parsed from the source CSV.</summary>
public sealed record Person(string FirstName, string LastName, string EmailAddress, string ZipCode);

/// <summary>Represents a person and the location attached to it.</summary>
public sealed record EnrichedPerson(
    string FirstName,
    string LastName,
    string EmailAddress,
    string ZipCode,
    string City,
    string State);

public sealed record Rejection(string RowDescription, string Reason);

/// <summary>Summarizes the people downloaded, rejected, and written for one run.</summary>
public sealed record RunResult(int TotalPeople, int RejectedPeople, int EnrichedPeople, IReadOnlyList<Rejection> Rejections)
{
    /// <summary>Groups rejection reasons for concise reporting.</summary>
    public IReadOnlyList<string> FormatRejectionSummary()
    {
        return Rejections
            .GroupBy(rejection => rejection.Reason, StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal)
            .Select(group => $"{group.Key}: {group.Count()}")
            .ToArray();
    }
}

public sealed class PeopleDownloadException : Exception
{
    public PeopleDownloadException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
