
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dag.Tests;

[TestClass]
public class Add_Tests
{
    [TestMethod]
    public void AddNode_AddsNodeToGraph()
    {
        var graph = new Dag<string>();

        graph.AddNode("CSCD 211");

        Assert.AreEqual(0, graph.GetDependencies("CSCD 211").ToArray().Length);
        Assert.AreEqual(0, graph.GetDependents("CSCD 211").ToArray().Length);
    }

    [TestMethod]
    public void AddNode_DuplicateNodeDoesNotThrowOrDuplicateEdges()
    {
        var graph = new Dag<string>();
        graph.AddNode("CSCD 211");
        graph.AddNode("CSCD 300");

        graph.AddNode("CSCD 211");
        graph.AddDependency("CSCD 211", "CSCD 300");
        graph.AddDependency("CSCD 211", "CSCD 300");

        CollectionAssert.AreEqual(
            new[] { "CSCD 211" },
            graph.GetDependencies("CSCD 300").ToArray());
        CollectionAssert.AreEqual(
            new[] { "CSCD 300" },
            graph.GetDependents("CSCD 211").ToArray());
    }
}
