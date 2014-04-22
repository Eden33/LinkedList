using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class GraphGenerator
    {
        private List<JunctionPoint> junctions = new List<JunctionPoint>();
        private List<Node> paths = new List<Node>();
        private Random rand = new Random();
        private int lastNodeID = (Node.ROOT_ID - 1);
        private int lastJunctionID = (JunctionPoint.ROOT_ID - 1);

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
            junctions.Clear();
            paths.Clear();
            lastNodeID = (Node.ROOT_ID - 1);
            lastJunctionID = (JunctionPoint.ROOT_ID - 1);

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

        private Node GenerateSinglePath()
        {
            //because rand.Next(1,2) upper bounds is exclusive we have with
            //one Node with this parametes between each junction
            int numNodes = rand.Next(1, 2);
            int firstId, lastId;
            firstId = lastId = (lastNodeID += 1);
            
            Node node = new Node(lastNodeID);
            paths.Add(node);
            for(int i = 0; i < (numNodes-1); i++)
            {
                node.InsertAtEnd(new Node(++lastNodeID));
            }
            //Console.WriteLine("Generated Node " + firstId + " to " + lastId + " added to paths at idx: " + (paths.Count - 1));
            //Console.WriteLine("Generated Node-Path: " + firstId);
         
            return node;            
        }

        public Node GetRandomNode()
        {
            Node found = null;
            int randNodeId = -1;
            if(paths.Count != 0)
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
}
