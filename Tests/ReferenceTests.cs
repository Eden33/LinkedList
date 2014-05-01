using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Graph.Model;

namespace Test
{
    [TestFixture(Description = "Some simple tests for myself.")]
    class ReferenceTests
    {
        [Test]
        public void testReferences()
        {
            Node dummy = new Node(1);
            Assert.AreEqual(dummy.Id, 1);
            List<Node> list = new List<Node>();
            list.Add(dummy);
            dummy = null;
            dummy = list.ElementAt(0);
            Assert.AreEqual(dummy.Id, 1);
        }
    }
}
