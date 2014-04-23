using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class RandomGenerator
    {
        protected List<JunctionPoint> junctions = new List<JunctionPoint>();
        protected List<Node> paths = new List<Node>();
        protected Random rand = new Random();
        protected int lastNodeID;
        protected int lastJunctionID;

        public RandomGenerator()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            junctions.Clear();
            paths.Clear();
            lastNodeID = (Node.ROOT_ID - 1);
            lastJunctionID = (JunctionPoint.ROOT_ID - 1);
        }

        protected Node GenerateSinglePath()
        {
            //because rand.Next(1,2) upper bounds is exclusive we have with
            //one Node with this parametes between each junction
            int numNodes = rand.Next(1, 2);
            int firstId, lastId;
            firstId = lastId = (lastNodeID += 1);

            Node node = new Node(lastNodeID);
            paths.Add(node);
            for (int i = 0; i < (numNodes - 1); i++)
            {
                node.InsertAtEnd(new Node(++lastNodeID));
            }
            //Console.WriteLine("Generated Node " + firstId + " to " + lastId + " added to paths at idx: " + (paths.Count - 1));
            //Console.WriteLine("Generated Node-Path: " + firstId);

            return node;
        }

        public INode GetRandomNode()
        {
            INode found = null;
            int randNodeId = -1;
            if (paths.Count != 0)
            {
                Node rootPath = paths.ElementAt(0);
                int searchId = JunctionPoint.SearchBuffer.InitSearchBuffer();
                randNodeId = rand.Next(Node.ROOT_ID, lastNodeID);
                Console.WriteLine("Start search rand node id: " + randNodeId + " from graph root.");
                found = rootPath.Find(randNodeId, searchId);
                if (found == null)
                {
                    //Console.WriteLine("Can not find next random Node with id: " + randNodeId);
                }
                JunctionPoint.SearchBuffer.ClearSearchBuffer(searchId);
                JunctionPoint.SearchBuffer.ResetSearchBuffer();
            }
            return found;
        }

        public int GetNumberOfNodes()
        {
            return lastNodeID;
        }

        public int GetNumberOfJunctions()
        {
            return junctions.Count;
        }
    }

    public class TreeGenerator : RandomGenerator
    {
        List<JunctionPoint> junctionPool = new List<JunctionPoint>();

        /// <summary>
        /// In general e tree is composed of knots (JunctionPoints) and edgeds (Node-Paths).
        /// This generation function generates a non balanced tree in which each knot contains max two ancestor edges.
        /// </summary>
        /// <param name="numPaths">number of edges to generate</param>
        public void GenerateRandomSimpleTree(int numPaths) 
        {
            Reset();

            for (int i = 0; i < numPaths; i++)
            {
                Node path = GenerateSinglePath();

                JunctionPoint startJunction = null;
                JunctionPoint endJunction = null;

                if (junctions.Count == 0)
                {
                    //this is the first Node-Path to be created
                    startJunction = new JunctionPoint(++lastJunctionID);
                    junctions.Add(startJunction);
                    junctionPool.Add(startJunction);
                }
                else
                {
                    int startJunctionIdx = rand.Next(0, junctionPool.Count);
                    startJunction = junctionPool.ElementAt(startJunctionIdx);
                    if (startJunction.NextPathCount >= 2)
                    {
                        junctionPool.RemoveAt(startJunctionIdx);
                    }
                }

                endJunction = new JunctionPoint(++lastJunctionID);
                junctions.Add(endJunction);
                junctionPool.Add(endJunction);

                path.InsertAtStart(startJunction);
                path.InsertAtEnd(endJunction);
                
            }
        }

        protected override void Reset()
        {
            base.Reset();
            junctionPool.Clear();
        }
    }
    public class GraphGenerator : RandomGenerator
    {

        /// <summary>
        /// Controls if root path maybe cyclic during random graph generation
        /// </summary>
        private bool rootPathMaybeCyclic = true;

        public bool RootMaybeCyclic 
        {
            set { rootPathMaybeCyclic = value; } 
        }

        public void GenerateRandomGraph(int numPaths)
        {
            Reset();

            for (int i = 0; i < numPaths; i++)
            {
                Node path = GenerateSinglePath();
                
                // generate the Junctions or choose exising ones in a random manner
                JunctionPoint startJunction = null;
                JunctionPoint endJunction = null;

                if (junctions.Count == 0)
                {
                    //this is the first Node-Path to be created
                    startJunction = new JunctionPoint(++lastJunctionID);
                    endJunction = new JunctionPoint(++lastJunctionID);
                    if (rootPathMaybeCyclic)
                    {
                        junctions.Add(startJunction);
                    }
                    junctions.Add(endJunction);
                }
                else
                {
                    //rand upper bound is exclusive, lower bound inclusive
                    startJunction = junctions.ElementAt(rand.Next(0, junctions.Count));
                    if (rand.NextDouble() > 0.5)
                    {
                        endJunction = junctions.ElementAt(rand.Next(0, junctions.Count));
                    }
                    else
                    {
                        endJunction = new JunctionPoint(++lastJunctionID);
                        junctions.Add(endJunction);
                    }
                }
                //Console.WriteLine("Connect to junctions start/end: " + startJunction.Id + "/" + endJunction.Id);

                // attach the junctions
                path.InsertAtStart(startJunction);
                path.InsertAtEnd(endJunction);
            }
            Console.WriteLine("Graph generated. Graph node count: " + GetNumberOfNodes() + " graph junction count: "+ GetNumberOfJunctions());
        }

    }
}
