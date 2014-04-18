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
            for (int i = 0; i < 100; i++ )
            {
                gen.GenerateRandomGraph(100);
                Node rand = gen.GetRandomNode();
                int searchId = JunctionPoint.SearchBuffer.InitSearchBuffer();
                Node rootNode = null;
                try
                {
                    rootNode = rand.Find(Node.ROOT_ID, searchId);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Check this.");
                }
                JunctionPoint.SearchBuffer.ClearSearchBuffer(searchId);
                Assert.IsNotNull(rootNode);
                Assert.AreEqual(rootNode.Value, Node.ROOT_ID);
            }
        }
        public void testFindNonCyclicRootNode()
        {
            gen.RootMaybeCyclic = false;
            for (int i = 0; i < 100; i++)
            {
                gen.GenerateRandomGraph(10);
                int searchId = JunctionPoint.SearchBuffer.InitSearchBuffer();
                Node rand = gen.GetRandomNode();
                rand.Find(Node.ROOT_ID, searchId);
                JunctionPoint.SearchBuffer.ClearSearchBuffer(searchId);

            }
        }
    }
}
