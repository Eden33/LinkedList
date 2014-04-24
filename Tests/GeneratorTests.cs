using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Graph.Generate;
using Graph.Model;

namespace Graph.Test
{
    public abstract class GeneratorTestBase
    {
        protected INode randomNode;

        protected void findRootsFromRandomStartPoint()
        {
            int searchIdRootNode = JunctionPoint.SearchBuffer.InitSearchBuffer();
            int searchIdRootJunction = JunctionPoint.SearchBuffer.InitSearchBuffer();
            INode rootNode = null;
            INode rootJunction = null;
            try
            {
                rootNode = randomNode.Find(Node.ROOT_ID, searchIdRootNode);
                rootJunction = randomNode.Find(JunctionPoint.ROOT_ID, searchIdRootJunction);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in findRootsFromRandomStartPoint(): " + e.Message);
            }
            JunctionPoint.SearchBuffer.ClearSearchBuffer(searchIdRootNode);
            JunctionPoint.SearchBuffer.ClearSearchBuffer(searchIdRootJunction);
            JunctionPoint.SearchBuffer.ResetSearchBuffer();

            Assert.IsNotNull(rootNode);
            Assert.IsNotNull(rootJunction);
            Assert.AreEqual(rootNode.Id, Node.ROOT_ID);
            Assert.AreEqual(rootJunction.Id, JunctionPoint.ROOT_ID);
        }
    }
    [TestFixture]
    public class GeneratorTests : GeneratorTestBase
    {
        GraphGenerator generator = new GraphGenerator();

        [Test]
        public void testFindPossibleCyclicRootNode()
        {
            generator.RootMaybeCyclic = true;
            for (int i = 0; i < 50; i++ )
            {
                generator.GenerateRandomGraph(500);
                randomNode = generator.GetRandomNode();
                Console.WriteLine("Start graph-search from node-id: " + randomNode.Id);
                findRootsFromRandomStartPoint();
            }
        }

        [Test]
        public void testFindNonCyclicRootNode()
        {
            generator.RootMaybeCyclic = false;
            for (int i = 0; i < 50; i++ )
            {
                generator.GenerateRandomGraph(500);
                randomNode = generator.GetRandomNode();
                Console.WriteLine("Start graph-search from node-id: " + randomNode.Id);
                findRootsFromRandomStartPoint();
            }
        }
    }

    [TestFixture]
    class TreeGeneratorTests : GeneratorTestBase
    {
        private TreeGenerator generator = new TreeGenerator();

        [Test]
        public void testFindRoots()
        {
            for(int i = 0; i < 50; i++)
            {
                generator.GenerateRandomBinaryTree(500);
                randomNode = generator.GetRandomNode();
                Console.WriteLine("Start tree-search from node-id: " + randomNode.Id);
                findRootsFromRandomStartPoint();
            }
        }

        [Test]
        public void testMaxTwoAncestors()
        {
            for (int i = 0; i < 50; i++)
            {
                generator.GenerateRandomBinaryTree(500);
                generator.Junctions.ForEach(delegate(JunctionPoint j)
                {
                    Assert.LessOrEqual(j.NextPathCount, 2);
                });
            }
        }
    }
}
