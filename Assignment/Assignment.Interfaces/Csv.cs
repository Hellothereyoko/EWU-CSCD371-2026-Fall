using System.Globalization;
using System.Text;

namespace Assignment.Interfaces;

public static class Csv
{
    /// <summary>Parses RFC-style CSV content, including quoted commas and escaped quotes.</summary>
    public static IReadOnlyList<string[]> Parse(string csv)
    {
        var rows = new List<string[]>();
        var row = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < csv.Length; i++)
        {
            var character = csv[i];
            if (character == '"' )
            {
                if (inQuotes && i + 1 < csv.Length && csv[i + 1] == '"')
                {
                    field.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (character == ',' && !inQuotes)
            {
                row.Add(field.ToString());
                field.Clear();
            }
            else if ((character == '\r' || character == '\n') && !inQuotes)
            {
                if (character == '\r' && i + 1 < csv.Length && csv[i + 1] == '\n')
                {
                    i++;
                }

                row.Add(field.ToString());
                field.Clear();
                if (row.Any(value => value.Length > 0))
                {
                    rows.Add(row.ToArray());
                }
                row.Clear();
            }
            else
            {
                field.Append(character);
            }
        }

        if (inQuotes)
        {
            throw new InvalidDataException("CSV contains an unterminated quoted field.");
        }

        if (field.Length > 0 || row.Count > 0)
        {
            row.Add(field.ToString());
            rows.Add(row.ToArray());
        }

        return rows;
    }

    public static string Escape(string value)
    {
        if (value.IndexOfAny([',', '"', '\r', '\n']) < 0)
        {
            return value;
        }

        return '"' + value.Replace("\"", "\"\"", StringComparison.Ordinal) + '"';
    }

    public static string FormatHeader(string[] header) =>
        string.Join(',', header.Select(Escape));

    public static string FormatRow(string[] values) =>
        string.Join(',', values.Select(Escape));

    public static bool IsValidPerson(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.FirstName) ||
            string.IsNullOrWhiteSpace(person.LastName) ||
            string.IsNullOrWhiteSpace(person.EmailAddress))
        {
            return false;
        }

        var email = person.EmailAddress.Trim();
        var at = email.IndexOf('@', StringComparison.Ordinal);
        return at > 0 &&
               at == email.LastIndexOf('@') &&
               at < email.Length - 1 &&
               email[(at + 1)..].Contains('.', StringComparison.Ordinal) &&
               !email.Any(char.IsWhiteSpace) &&
               person.ZipCode.Trim().Length == 5 &&
               person.ZipCode.Trim().All(char.IsAsciiDigit);
    }

    public static string PersonInvalidReason(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.FirstName) ||
            string.IsNullOrWhiteSpace(person.LastName))
        {
            return "Missing name";
        }

        var email = person.EmailAddress.Trim();
        var at = email.IndexOf('@', StringComparison.Ordinal);
        if (at <= 0 ||
            at != email.LastIndexOf('@') ||
            at >= email.Length - 1 ||
            !email[(at + 1)..].Contains('.', StringComparison.Ordinal) ||
            email.Any(char.IsWhiteSpace))
        {
            return "Invalid email";
        }

        var zip = person.ZipCode.Trim();
        if (zip.Length != 5 || !zip.All(char.IsAsciiDigit))
        {
            return "Invalid ZIP code";
        }

        return "Invalid person";
    }

    public static bool IsValidLocation(string city, string state) =>
        !string.IsNullOrWhiteSpace(city) &&
        state.Length == 2 &&
        state.All(char.IsAsciiLetter) &&
        state.All(char.IsUpper);
}
