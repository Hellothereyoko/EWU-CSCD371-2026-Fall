using System.Globalization;

namespace Assignment8.PeopleImporter;

public delegate string? PersonValidator(Person person);

/// <summary>
/// Imports CSV records from a TextReader. The importer owns the reader passed to it
/// and disposes that reader when <see cref="Dispose"/> is called.
/// </summary>
#pragma warning disable CA1724
public sealed class PeopleImporter : IDisposable
#pragma warning restore CA1724
{
    private static readonly string[] Header =
    [
        "Id",
        "FirstName",
        "LastName",
        "Email",
        "StreetAddress",
        "City",
        "State",
        "Zip"
    ];

    private readonly TextReader reader;
    private bool disposed;

    public PeopleImporter(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader, nameof(reader));
        this.reader = reader;
    }

#pragma warning disable CA1062
    public IReadOnlyList<Person> Import(
        ImportMode mode,
        Action<InvalidPersonRecordException>? invalidRecordCallback = null,
        PersonValidator? validationDelegate = null)
    {
        ThrowIfDisposed();
        if (!Enum.IsDefined(mode))
        {
            throw new ArgumentOutOfRangeException(nameof(mode), mode, "The import mode is not defined.");
        }

        if (mode == ImportMode.SkipInvalid)
        {
            ArgumentNullException.ThrowIfNull(invalidRecordCallback, nameof(invalidRecordCallback));
        }

        Action<InvalidPersonRecordException>? skipCallback =
            mode == ImportMode.SkipInvalid
                ? invalidRecordCallback
                : null;

        var header = reader.ReadLine()
            ?? throw CreateLineException(1, string.Empty, new FormatException("The CSV header is missing."));

        ValidateHeader(header);

        var accepted = new List<Person>();
        var seenIds = new HashSet<int>();
        var lineNumber = 1;

        string? rawLine;
        while ((rawLine = reader.ReadLine()) is not null)
        {
            lineNumber++;

            Person person;
            try
            {
                person = ParsePerson(rawLine, lineNumber, seenIds);
            }
            catch (Exception exception) when (
                exception is FormatException
                or ArgumentException
                or OverflowException
                or InvalidOperationException)
            {
                var invalid = CreateLineException(lineNumber, rawLine, exception);
                if (mode == ImportMode.FailFast)
                {
                    throw invalid;
                }

                skipCallback!(invalid);
                continue;
            }

            string? violation = validationDelegate?.Invoke(person);
            if (violation is not null)
            {
                var invalid = CreateLineException(
                    lineNumber,
                    rawLine,
                    new InvalidOperationException(violation));

                if (mode == ImportMode.FailFast)
                {
                    throw invalid;
                }

                skipCallback!(invalid);
                continue;
            }

            accepted.Add(person);
        }

        return accepted;
    }
#pragma warning restore CA1062

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        reader.Dispose();
        disposed = true;
    }

    private static Person ParsePerson(string rawLine, int lineNumber, HashSet<int> seenIds)
    {
        var fields = rawLine.Split(',');
        if (fields.Length != Header.Length)
        {
            throw new FormatException(
                $"Expected {Header.Length} fields but found {fields.Length}.");
        }

        for (var index = 0; index < fields.Length; index++)
        {
            fields[index] = fields[index].Trim();
        }

        if (!int.TryParse(fields[0], NumberStyles.None, CultureInfo.InvariantCulture, out var id))
        {
            throw new FormatException("Id must be a base-10 integer.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

        if (!seenIds.Add(id))
        {
            throw new InvalidOperationException($"Id {id.ToString(CultureInfo.InvariantCulture)} is duplicated.");
        }

        if (Array.Exists(fields, string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Required CSV values cannot be missing.", nameof(rawLine));
        }

        if (!fields[3].Contains('@', StringComparison.Ordinal))
        {
            throw new ArgumentException("Email must contain an '@' character.", nameof(rawLine));
        }

        if (fields[6].Length != 2)
        {
            throw new ArgumentException("State must contain exactly two characters.", nameof(rawLine));
        }

        if (!fields[7].All(char.IsAsciiDigit))
        {
            throw new ArgumentException("Zip must contain only ASCII digits.", nameof(rawLine));
        }

        var address = new Address(fields[4], fields[5], fields[6], fields[7]);
        return new Person(id, fields[1], fields[2], fields[3], address);
    }

    private static void ValidateHeader(string header)
    {
        var fields = header.Split(',');
        if (fields.Length != Header.Length)
        {
            throw CreateLineException(
                1,
                header,
                new FormatException($"The header must contain {Header.Length} fields."));
        }

        for (var index = 0; index < Header.Length; index++)
        {
            if (!string.Equals(fields[index].Trim(), Header[index], StringComparison.Ordinal))
            {
                throw CreateLineException(
                    1,
                    header,
                    new FormatException(
                        $"Header field {index + 1} must be '{Header[index]}'."));
            }
        }
    }

    private void ThrowIfDisposed()
        => ObjectDisposedException.ThrowIf(disposed, this);

    private static InvalidPersonRecordException CreateLineException(
        int lineNumber,
        string rawLine,
        Exception innerException)
    {
        return new InvalidPersonRecordException(
            $"Invalid person record on line {lineNumber.ToString(CultureInfo.InvariantCulture)}.",
            lineNumber,
            rawLine,
            innerException);
    }
}
