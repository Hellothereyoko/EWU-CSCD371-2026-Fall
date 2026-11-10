namespace Assignment.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment;
using Assignment.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;


/// <summary>
/// This class contains unit tests for the SampleDataAsync class, which implements the IAsyncSampleData interface.
/// </summary>
[TestClass] 
public class SampleDataAsyncTests
{
    
    /// <summary>
    /// This test method verifies that the CsvRows property of the SampleDataAsync class returns the expected number of rows from the "People.csv" file.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task CsvRows_ShouldReturnExpectedNumberOfRows()
    {
        Assignment.SampleDataAsync sampleData = new SampleDataAsync();
        int expectedRowCount = 50; 
        int actualRowCount = 0;

        await foreach (var row in sampleData.CsvRows)
        {
            actualRowCount++;
        }

        Assert.AreEqual(expectedRowCount, actualRowCount);
    }


    /// <summary>
    /// This test method verifies that the GetUniqueSortedListOfStatesGivenCsvRows method of the SampleDataAsync class returns the expected unique sorted list of states from the "People.csv" file.
    /// </summary>
    /// <returns></returns>
    [TestMethod]    
    public async Task GetUniqueSortedListOfStatesGivenCsvRows_ShouldReturnExpectedStates()
    {
        Assignment.SampleDataAsync sampleData = new SampleDataAsync();
        var expectedStates = new List<string> { "AL", "AZ", "CA","DC","FL","GA", "IN", "KS", "LA", "MD", "MN", "MO", "MT", "NC", "NE", "NH", "NV", "NY", "OR", "PA", "SC", "TN", "TX", "UT", "VA", "WA", "WV" }; 
        var actualStates = new List<string>();

        await foreach (var state in sampleData.GetUniqueSortedListOfStatesGivenCsvRows())
        {
            actualStates.Add(state);
        }

        Assert.AreSequenceEqual(expectedStates, actualStates);
    }


    /// <summary>
    /// This test method verifies that the GetAggregateSortedListOfStatesUsingCsvRows method of the SampleDataAsync class returns the expected aggregate string of unique sorted states from the "People.csv" file.
    /// </summary>
    [TestMethod]
    public void GetAggregateSortedListOfStatesUsingCsvRows_ShouldReturnExpectedAggregateString()
    {
        Assignment.SampleDataAsync sampleData = new SampleDataAsync();
        string expectedAggregate = "AL,AZ,CA,DC,FL,GA,IN,KS,LA,MD,MN,MO,MT,NC,NE,NH,NV,NY,OR,PA,SC,TN,TX,UT,VA,WA,WV";
        string actualAggregate = sampleData.GetAggregateSortedListOfStatesUsingCsvRows();       

        Assert.AreEqual(expectedAggregate, actualAggregate);
    }


    /// <summary>
    /// This test method verifies that the People property of the SampleDataAsync class returns the expected number of IPerson objects from the "People.csv" file.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task EnumerableOfPeople_ShouldReturnExpectedNumberOfPeople()
    {
        Assignment.SampleDataAsync sampleData = new SampleDataAsync();
        int expectedCount = 50; 
        int actualCount = 0;

        await foreach (var person in sampleData.People)
        {
            actualCount++;
        }

        Assert.AreEqual(expectedCount, actualCount);
    }


    // Missing coverage for lines:

        // 102-110: FilterByEmailAddress(Predicate<string> filter) method
        // 180-183: GetAggregateListOfStatesGivenPeopleCollectionAsync(IAsyncEnumerable<IPerson> people) method
}   