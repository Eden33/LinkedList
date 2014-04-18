using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    class GraphGenerator
    {
        private List<JunctionPoint> junctions = new List<JunctionPoint>();
        private List<Node> paths = new List<Node>();
        private Random rand = new Random();
        private int lastNodeID = Node.ROOT_ID;

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
            lastNodeID = Node.ROOT_ID - 1;

            for (int i = 0; i < numPaths; i++)
            {
                Node path = GenerateSinglePath();
                
                // generate the Junctions or choose exising ones in a random manner
                JunctionPoint startJunction = null;
                JunctionPoint endJunction = null;
                if (junctions.Count == 0)
                {
                    //this is the first Node-Path to be created
                    startJunction = new JunctionPoint();
                    endJunction = new JunctionPoint();
                    if (rootPathMaybeCyclic)
                    {
                        junctions.Add(startJunction);
                    }
                    junctions.Add(endJunction);
                }
                else
                {
                    startJunction = junctions.ElementAt(rand.Next(0, (junctions.Count - 1)));
                    if (rand.NextDouble() > 0.5)
                    {
                        endJunction = junctions.ElementAt(rand.Next(0, (junctions.Count - 1)));
                    }
                    else
                    {
                        endJunction = new JunctionPoint();
                        junctions.Add(endJunction);
                    }
                }

                // attach the junctions
                path.InsertAtStart(startJunction);
                path.InsertAtEnd(endJunction);
            }
        }

        private Node GenerateSinglePath()
        {
            int numNodes = rand.Next(1, 50);
            Node node = new Node(++lastNodeID);
            paths.Add(node);
            for(int i = 0; i < (numNodes-1); i++)
            {
                node.InsertAtEnd(new Node(++lastNodeID));
            }
            return node;            
        }

        public Node GetRandomNode()
        {
            if(paths.Count != 0)
            {
                Node rootPath = paths.ElementAt(0);
                return rootPath.Find(rand.Next(0, lastNodeID));
            }
            return null;
        }
    }
}
