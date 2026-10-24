
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

    [TestMethod]
    public void AddDependency_MissingNodesThrows()
    {
        var graph = new Dag<int>();

        var ex = Assert.Throws<InvalidOperationException>(() => graph.AddDependency(1, 2));
        Assert.AreEqual("Both nodes must exist in the graph.", ex.Message);
    }

    [TestMethod]
    public void AddDependency_WouldCreateCycleThrows()
    {
        var graph = new Dag<int>();

        graph.AddNode(0);
        graph.AddNode(1);
        graph.AddNode(2);

        graph.AddDependency(0, 1);
        graph.AddDependency(1, 2);

        var ex = Assert.Throws<InvalidOperationException>(() => graph.AddDependency(2, 0));
        Assert.AreEqual("Adding this dependency would create a cycle.", ex.Message);
    }
}
