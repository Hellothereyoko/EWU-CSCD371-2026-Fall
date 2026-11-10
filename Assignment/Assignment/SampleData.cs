using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment;

public class SampleData : ISampleData
{
    // 1.
    public IEnumerable<string> CsvRows
    {
        get
        {
            return File.ReadLines("People.csv").Skip(1);
        }
    }

    // 2.
    public IEnumerable<string> GetUniqueSortedListOfStatesGivenCsvRows()
    {
        return CsvRows
        .Select(row => row.Split(',')[5])
        .Distinct()
        .OrderBy(state => state);
    }

    // 3.
    public string GetAggregateSortedListOfStatesUsingCsvRows()
    {
       string[] states = GetUniqueSortedListOfStatesGivenCsvRows().ToArray();
        return string.Join(",", states);
    }

    // 4.
    public IEnumerable<IPerson> People
    {
        get
        {
            return CsvRows.Select(row =>
            {
                var columns = row.Split(',');
                return new Person
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    EmailAddress = columns[2],
                    Address = new Address
                    {
                        StreetAddress = columns[3],
                        City = columns[4],
                        State = columns[5],
                        Zip = columns[6]
                    }
                };
            })
            .OrderBy(p => p.Address.State)
            .ThenBy(p => p.Address.City)
            .ThenBy(p => p.Address.Zip);
        }
    }

    // 5.
    public IEnumerable<(string FirstName, string LastName)> FilterByEmailAddress(Predicate<string> filter)
    {
        return People
        .Where(p => filter(p.EmailAddress))
        .Select(p => (p.FirstName, p.LastName));
    }

    // 6.
    public string GetAggregateListOfStatesGivenPeopleCollection(IEnumerable<IPerson> people)
    {
        return people
        .Select(p => p.Address.State)
        .Distinct()
        .OrderBy(state => state)
        .Aggregate(
            (current, next) => $"{current},{next}");
    }

internal class Person : IPerson
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string EmailAddress { get; set; } = "";

    public IAddress Address { get; set; } =
        new Address();
}
internal class Address : IAddress
{
    public string StreetAddress { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Zip { get; set; } = "";


}