using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dag;
using System;



namespace Dag.Tests

{

[TestClass]
    public class Get_Tests{

        //Will function as the backend for Dag Class to implement Get Tests
        [TestMethod]
        public void GetDependencies_ThrowsExceptionOnMissingElement()
        {
            var dag = new Dag<int>();
            var ex = Assert.Throws<InvalidOperationException>(() => dag.GetDependencies(5));

            Assert.AreEqual("The node does not exist in the graph.", ex.Message);
        }
        

        [TestMethod]
        public void GetDependents_ThrowsExceptionOnMissingElement()
        {
            var dag = new Dag<int>();
            var ex = Assert.Throws<InvalidOperationException>(() => dag.GetDependents(5));

            Assert.AreEqual("The node does not exist in the graph.", ex.Message);
        }

    }
}
