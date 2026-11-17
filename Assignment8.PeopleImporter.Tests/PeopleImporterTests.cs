using System.Text;

namespace Assignment8.ResilientImporter.Tests;

public sealed class PeopleImporterTests
{
    private const string Header =
        "Id,FirstName,LastName,Email,StreetAddress,City,State,Zip";

    [Fact]
    public void ImportSucceedsForValidStringReader()
    {
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "1,Ada,Lovelace,ada@example.com,12 Math St,London,WA,99001"));

        var people = importer.Import(ImportMode.FailFast);

        var person = Assert.Single(people);
        Assert.Equal(1, person.Id);
        Assert.Equal("Ada", person.FirstName);
        Assert.Equal("Lovelace", person.LastName);
        Assert.Equal("ada@example.com", person.Email);
        Assert.Equal("12 Math St", person.Address.StreetAddress);
        Assert.Equal("London", person.Address.City);
        Assert.Equal("WA", person.Address.State);
        Assert.Equal("99001", person.Address.Zip);
    }

    [Fact]
    public void ImportSucceedsForTemporaryFileAndDeletesIt()
    {
        var path = Path.Combine(Path.GetTempPath(), $"people-{Guid.NewGuid():N}.csv");
        try
        {
            File.WriteAllText(
                path,
                $"{Header}{Environment.NewLine}" +
                "2,Grace,Hopper,grace@example.com,1 Navy Way,Arlington,VA,22201",
                new UTF8Encoding(false));

            using var reader = new StreamReader(path);
            using var importer = new PeopleImporter(reader);

            var people = importer.Import(ImportMode.FailFast);

            Assert.Equal(2, Assert.Single(people).Id);
        }
        finally
        {
            File.Delete(path);
        }

        Assert.False(File.Exists(path));
    }

    [Fact]
    public void ImportSucceedsForAssignment7PeopleCsv()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "People.csv");
        using var reader = new StreamReader(path);
        using var importer = new PeopleImporter(reader);

        var people = importer.Import(ImportMode.FailFast);

        Assert.Equal(50, people.Count);
        Assert.Equal(1, people[0].Id);
        Assert.Equal(50, people[49].Id);
    }

    [Fact]
    public void FailFastThrowsAtFirstBadLineAndWrapsInnerException()
    {
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            $"bad,Bad,Record,bad@example.com,1 Main St,City,WA,98001{Environment.NewLine}" +
            "3,Good,Record,good@example.com,2 Main St,City,OR,97001"));

        var exception = Assert.Throws<InvalidPersonRecordException>(
            () => importer.Import(ImportMode.FailFast));

        Assert.Equal(2, exception.LineNumber);
        Assert.StartsWith("bad,", exception.RawLine, StringComparison.Ordinal);
        Assert.IsType<FormatException>(exception.InnerException);
    }

    [Fact]
    public void SkipInvalidReturnsValidPeopleAndReportsEveryRejectedRecord()
    {
        var records = new List<InvalidPersonRecordException>();
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            $"1,Ada,Lovelace,ada@example.com,12 Math St,London,WA,99001{Environment.NewLine}" +
            $"1,Duplicate,Lovelace,dup@example.com,1 Street,City,WA,99001{Environment.NewLine}" +
            $"not-an-id,Missing,Email,bad,1 Street,City,WA,99001{Environment.NewLine}" +
            $"too,few,fields{Environment.NewLine}" +
            "4,,Hopper,grace@example.com,1 Navy Way,Arlington,VA,22201"));

        var people = importer.Import(
            ImportMode.SkipInvalid,
            exception => records.Add(exception));

        Assert.Equal(1, Assert.Single(people).Id);
        Assert.Equal(4, records.Count);
        Assert.Equal([3, 4, 5, 6], records.Select(record => record.LineNumber));
        Assert.All(records, record => Assert.NotNull(record.InnerException));
    }

    [Fact]
    public void DuplicateIdIsRejectedAsInvalidOperationException()
    {
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            $"7,One,Person,one@example.com,1 Main St,City,WA,98001{Environment.NewLine}" +
            "7,Two,Person,two@example.com,2 Main St,City,WA,98001"));

        var exception = Assert.Throws<InvalidPersonRecordException>(
            () => importer.Import(ImportMode.FailFast));

        Assert.Equal(3, exception.LineNumber);
        Assert.IsType<InvalidOperationException>(exception.InnerException);
    }

    [Fact]
    public void RejectedRecordDoesNotReserveItsIdForSkipInvalidImport()
    {
        var records = new List<InvalidPersonRecordException>();
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            $"12,Missing,Email,bad,1 Street,City,WA,99001{Environment.NewLine}" +
            "12,Valid,Person,valid@example.com,2 Street,City,WA,99002"));

        var people = importer.Import(
            ImportMode.SkipInvalid,
            records.Add);

        Assert.Equal(12, Assert.Single(people).Id);
        Assert.Single(records);
    }

    [Fact]
    public void MissingRequiredValuePreservesArgumentException()
    {
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "8, ,Lovelace,ada@example.com,12 Math St,London,WA,99001"));

        var exception = Assert.Throws<InvalidPersonRecordException>(
            () => importer.Import(ImportMode.FailFast));

        Assert.IsType<ArgumentException>(exception.InnerException);
    }

    [Fact]
    public void ConstructorAndMethodArgumentsHaveCorrectNames()
    {
        Assert.Equal(
            "reader",
            Assert.Throws<ArgumentNullException>(() => new PeopleImporter(null!)).ParamName);

        using var importer = new PeopleImporter(CreateReader(Header));

        Assert.Equal(
            "invalidRecordCallback",
            Assert.Throws<ArgumentNullException>(
                () => importer.Import(ImportMode.SkipInvalid)).ParamName);

        Assert.Equal(
            "mode",
            Assert.Throws<ArgumentOutOfRangeException>(
                () => importer.Import((ImportMode)999)).ParamName);

        Assert.Equal(
            "firstName",
            Assert.Throws<ArgumentException>(
                () => new Person(1, " ", "Lovelace", "ada@example.com", new Address("Main", "London", "WA", "99001"))).ParamName);

        Assert.Equal(
            "address",
            Assert.Throws<ArgumentNullException>(
                () => new Person(1, "Ada", "Lovelace", "ada@example.com", null!)).ParamName);
    }

    [Fact]
    public void ReaderIOExceptionPropagatesAsSameInstance()
    {
        var ioException = new IOException("reader failed");
        using var reader = new ThrowingTextReader(ioException);
        using var importer = new PeopleImporter(reader);

        var thrown = Assert.Throws<IOException>(
            () => importer.Import(ImportMode.FailFast));

        Assert.Same(ioException, thrown);
    }

    [Fact]
    public void CallbackExceptionPropagatesAsSameInstance()
    {
        var callbackException = new InvalidOperationException("callback failed");
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "bad,Bad,Record,bad,1 Street,City,WA,99001"));

        var thrown = Assert.Throws<InvalidOperationException>(
            () => importer.Import(
                ImportMode.SkipInvalid,
                (Action<InvalidPersonRecordException>)(_ => throw callbackException)));

        Assert.Same(callbackException, thrown);
    }

    [Fact]
    public void DisposesReaderAfterSuccessfulImport()
    {
        using var reader = new TrackingTextReader(Header);
        var importer = new PeopleImporter(reader);

        importer.Import(ImportMode.FailFast);
        importer.Dispose();

        Assert.True(reader.IsDisposed);
        importer.Dispose();
        Assert.True(reader.IsDisposed);
    }

    [Fact]
    public void DisposesReaderAfterFailedImport()
    {
        using var reader = new TrackingTextReader(
            $"{Header}{Environment.NewLine}bad,Person");
        var importer = new PeopleImporter(reader);

        Assert.Throws<InvalidPersonRecordException>(
            () => importer.Import(ImportMode.FailFast));

        importer.Dispose();

        Assert.True(reader.IsDisposed);
    }

    [Fact]
    public void UseAfterDisposalThrowsObjectDisposedException()
    {
        using var reader = new TrackingTextReader(Header);
        var importer = new PeopleImporter(reader);
        importer.Dispose();

        Assert.Throws<ObjectDisposedException>(
            () => importer.Import(ImportMode.FailFast));
    }

    [Fact]
    public void CustomValidationDelegateIsHandledInSelectedMode()
    {
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "9,Good,Person,good@example.com,1 Main St,City,WA,98001"));

        var exception = Assert.Throws<InvalidPersonRecordException>(
            () => importer.Import(
                ImportMode.FailFast,
                validationDelegate: _ => "Rejected by caller"));

        Assert.Equal(2, exception.LineNumber);
        Assert.IsType<InvalidOperationException>(exception.InnerException);
    }

    [Fact]
    public void CustomValidationDelegateIsHandledInSkipInvalidMode()
    {
        var records = new List<InvalidPersonRecordException>();
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "13,Rejected,Person,rejected@example.com,1 Main St,City,WA,98001"));

        var people = importer.Import(
            ImportMode.SkipInvalid,
            records.Add,
            _ => "Rejected by caller");

        Assert.Empty(people);
        Assert.Equal(2, Assert.Single(records).LineNumber);
    }

    [Fact]
    public void UnexpectedValidationExceptionPropagatesUnwrapped()
    {
        var unexpected = new UnexpectedValidationException("unexpected");
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "10,Good,Person,good@example.com,1 Main St,City,WA,98001"));

        var thrown = Assert.Throws<UnexpectedValidationException>(
            () => importer.Import(
                ImportMode.FailFast,
                validationDelegate: _ => throw unexpected));

        Assert.Same(unexpected, thrown);
    }

    [Fact]
    public void WhenFilterDoesNotCatchUnexpectedRecordException()
    {
        var unexpected = new UnexpectedValidationException("not a record error");
        using var importer = new PeopleImporter(CreateReader(
            $"{Header}{Environment.NewLine}" +
            "11,Good,Person,good@example.com,1 Main St,City,WA,98001"));

        var thrown = Assert.Throws<UnexpectedValidationException>(
            () => importer.Import(
                ImportMode.FailFast,
                validationDelegate: _ => throw unexpected));

        Assert.Same(unexpected, thrown);
    }

    private static StringReader CreateReader(string text)
        => new StringReader(text);

    private sealed class UnexpectedValidationException : Exception
    {
        public UnexpectedValidationException()
        {
        }

        public UnexpectedValidationException(string message)
            : base(message)
        {
        }

        public UnexpectedValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    private sealed class ThrowingTextReader(Exception exception) : TextReader
    {
        public override string? ReadLine()
            => throw exception;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    private sealed class TrackingTextReader(string text) : TextReader
    {
        private readonly Queue<string?> lines =
            new(text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n'));

        public bool IsDisposed { get; private set; }

        public override string? ReadLine()
        {
            if (lines.Count == 0)
            {
                return null;
            }

            return lines.Dequeue();
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
