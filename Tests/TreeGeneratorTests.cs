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
    class TreeGeneratorTests
    {
        private TreeGenerator gen = new TreeGenerator();

        [Test]
        public void testFindRoots()
        {
            for(int i = 0; i < 50; i++)
            {
                gen.GenerateRandomSimpleTree(60);
                INode rand = gen.GetRandomNode();
                int searchIdRootNode = JunctionPoint.SearchBuffer.InitSearchBuffer();
                int searchIdRootJunction = JunctionPoint.SearchBuffer.InitSearchBuffer();
                INode rootNode = null;
                INode rootJunction = null;
                try
                {
                    rootNode = rand.Find(Node.ROOT_ID, searchIdRootNode);
                    rootJunction = rand.Find(JunctionPoint.ROOT_ID, searchIdRootJunction);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception cuaght in testFindRoots(): " + e.Message);
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
    }
}
