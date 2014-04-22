using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LinkedList;

namespace Tests
{
    [TestFixture]
    public class GraphGeneratorTests
    {
        private GraphGenerator gen = new GraphGenerator();
        [Test]
        public void testFindPossibleCyclicRootNode()
        {
            gen.RootMaybeCyclic = true;
            findRootFromRandomStartPointInGraph();
        }

        [Test]
        public void testFindNonCyclicRootNode()
        {
            gen.RootMaybeCyclic = false;
            findRootFromRandomStartPointInGraph();
        }

        private void findRootFromRandomStartPointInGraph() 
        {
            for (int i = 0; i < 10; i++)
            {
                gen.GenerateRandomGraph(500);
                Node rand = gen.GetRandomNode();
                int searchId = JunctionPoint.SearchBuffer.InitSearchBuffer();
                Node rootNode = null;
                try
                {
                    rootNode = rand.Find(Node.ROOT_ID, searchId);                        
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception cuaght in testFindNonCyclicRootNode(): " + e.Message);
                }
                JunctionPoint.SearchBuffer.ClearSearchBuffer(searchId);
                JunctionPoint.SearchBuffer.ResetSearchBuffer();

                Assert.IsNotNull(rootNode);
                Assert.AreEqual(rootNode.Value, Node.ROOT_ID);
            }
        }
    }
}
