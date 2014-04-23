using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedList
{
    public interface INode
    {
        int Id { get; }
        INode FindNext(int idToFind, int searchId);
        INode FindPrev(int idToFind, int searchId);
        INode Find(int idToFind, int searchId);
    }

    public class Node : INode
    {
        private int id;
        private INode next = null;
        private INode prev = null;
        public static readonly int ROOT_ID = 1;

        public int Id 
        {
            get { return id; } 
        }

        public Node(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Inserts the Node at the end of this Node-Path and returns the Node.
        /// </summary>
        /// <param name="nodeToAdd">The node to add.</param>
        /// <returns>The added Node.</returns>
        public Node InsertAtEnd(Node nodeToAdd)
        {
            if (next == null)
            {
                nodeToAdd.prev = this;
                next = nodeToAdd;
                return nodeToAdd;
            }
            else if(next is Node)
            {
                Node nextNode = (Node) next;
                return nextNode.InsertAtEnd(nodeToAdd);
            }

            //else next is a JunctionPoint, insert it bevor
            nodeToAdd.prev = this;
            nodeToAdd.next = next;
            return nodeToAdd;
        }

        /// <summary>
        /// Add a JunctionPoint to the end of this Node-Path.
        /// At the end of each Node-Path exactly one JunctionPoint can be added.
        /// </summary>
        /// <param name="j"></param>
        /// <returns>The JunctionPoint if added successfully, null if allready a JunctionPoint has been added to the end of this Node-Path.</returns>
        public JunctionPoint InsertAtEnd(JunctionPoint j)
        {
            if (next == null)
            {
                next = j;
                j.AddPrevPath(this);
                return j;
            }
            else if (next is Node)
            {
                Node nextNode = (Node)next;
                return nextNode.InsertAtEnd(j);
            }

            // else path allready has an End-JunctionPoint set
            return null;
        }

        /// <summary>
        /// Add a JunctionPoint to the start of this Node-Path.
        /// At the start of each Node-Path exactly one JunctionPoint can be added.
        /// </summary>
        /// <param name="j"></param>
        /// <returns>The JunctionPoint if added successfully, null if allready a JunctionPoint has been added to the start of this Node-Path.</returns>
        public JunctionPoint InsertAtStart(JunctionPoint j)
        {
            if (prev == null)
            {
                prev = j;
                j.AddNextPath(this);
                return j;
            }
            else if (prev is Node)
            {
                Node prevNode = (Node)prev;
                return prevNode.InsertAtStart(j);
            }

            //else path allready has a Start-JunctionPoint set
            return null;
        }

        #region Graph Search

        /// <summary>
        /// Search in Next-Direction, the value of current INode is checked too.
        /// </summary>
        /// <param name="idToFind">The id to be searched for.</param>
        /// <returns>The INode if found, otherwise null</returns>
        public INode FindNext(int idToFind, int searchId)
        {
            if (id.Equals(idToFind))
                return (INode) this;

            if(next is Node) 
            {
                return next.FindNext(idToFind, searchId);          
            }
            else if (next is JunctionPoint)
            {
                JunctionPoint nextJunction = (JunctionPoint) next;
                return nextJunction.Find(idToFind, searchId);
            }
            else return null;
        }

        /// <summary>
        /// Search in Prev-Direction, the value of current INode is checked too.
        /// </summary>
        /// <param name="idToFind">The id to be searched for.</param>
        /// <returns>The INode if found, otherwise null</returns>
        public INode FindPrev(int idToFind, int searchId)
        {
            if (id.Equals(idToFind))
                return this;

            if(prev is Node) 
            {
                return prev.FindPrev(idToFind, searchId);            
            }
            else if (prev is JunctionPoint)
            {
                JunctionPoint prevJunction = (JunctionPoint) prev;
                return prevJunction.Find(idToFind, searchId);
            }
            else return null;
        }

        public INode Find(int idToFind, int searchId)
        {
            INode found = FindNext(idToFind, searchId);
            if (found == null)
            {
                found = FindPrev(idToFind, searchId);
            }
            return found;
        }

        #endregion
    }

    public class JunctionPoint : INode
    {
        private List<Node> nextPath = new List<Node>();
        private List<Node> prevPath = new List<Node>();
        public static readonly int ROOT_ID = 1000000;
        private int id;

        public int Id {
            get { return id; }
        }

        public int NextPathCount {
            get { return nextPath.Count; }
        }

        public JunctionPoint(int id)
        {
            this.id = id;
        }

        public INode FindNext(int idToFind, int searchId)
        {
            if (this.id.Equals(idToFind))
            {
                return this;
            }

            if (SearchBuffer.JunctionVisitedAllready(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_NEXT))
            {
                return null;
            }
            SearchBuffer.MarkJunctionVisited(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_NEXT);
            //Console.WriteLine("Search reached Junction " + this.id + " in next direction.");

            INode found = null;
            foreach (Node n in nextPath)
            {
                found = n.FindNext(idToFind, searchId);
                if (found != null)
                    return found;
            }
            return null;
        }

        public INode FindPrev(int idToFind, int searchId)
        {
            if (this.id.Equals(idToFind))
            {
                return this;
            }

            if (SearchBuffer.JunctionVisitedAllready(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_PREV))
            {
                return null;
            }
            SearchBuffer.MarkJunctionVisited(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_PREV);
            //Console.WriteLine("Search reached Junction " + this.id + " in prev direction.");

            INode found = null;
            foreach (Node n in prevPath)
            {
                found = n.FindPrev(idToFind, searchId);
                if (found != null)
                    return found;
            }
            return null;
        }

        public INode Find(int idToFind, int searchId)
        {
            if (this.id.Equals(idToFind))
            {
                return this;
            }
            INode found = null;
            found = FindNext(idToFind, searchId);
            if (found == null)
            {
                found = FindPrev(idToFind, searchId);
            }
            return found;
        }

        public void AddPrevPath(Node node)
        {
            prevPath.Add(node);
        }
        public void AddNextPath(Node node)
        {
            nextPath.Add(node);
        }

        public class SearchBuffer
        {
            private static int searchBufferID = 0;
            private static Dictionary<int, Dictionary<int, DirectionMarker>> visitedJunctions
                    = new Dictionary<int, Dictionary<int, DirectionMarker>>();

            public enum SearchDirection
            {
                SEARCH_NEXT,
                SEARCH_PREV
            }

            public class DirectionMarker
            {
                public bool searchNext = false;
                public bool searchPrev = false;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static int InitSearchBuffer()
            {
                searchBufferID += 1;
                if (visitedJunctions.ContainsKey(searchBufferID))
                {
                    throw new SystemException("visitedJunctions alleready contains this ID!");
                }
                else
                {
                    visitedJunctions.Add(searchBufferID, new Dictionary<int, DirectionMarker>());
                }
                return searchBufferID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void ClearSearchBuffer(int id)
            {
                visitedJunctions.Remove(id);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void ResetSearchBuffer()
            {
                searchBufferID = 0;
                visitedJunctions.Clear();
            }

            public static bool JunctionVisitedAllready(int searchId, int junctionId, SearchDirection dir)
            {
                Dictionary<int, DirectionMarker> dict = null;
                if (visitedJunctions.TryGetValue(searchId, out dict))
                {
                    DirectionMarker marker;
                    if (dict.TryGetValue(junctionId, out marker))
                    {
                        if (dir.Equals(SearchDirection.SEARCH_NEXT))
                        {
                            if (marker.searchNext)
                                return true;
                        }
                        if (dir.Equals(SearchDirection.SEARCH_PREV))
                        {
                            if (marker.searchPrev)
                                return true;
                        }
                    }
                }
                return false;
            }
                        
            public static void MarkJunctionVisited(int searchId, int junctionId, SearchDirection dir)
            {
                Dictionary<int, DirectionMarker> dict = null;
                if(visitedJunctions.TryGetValue(searchId, out dict))
                {
                    DirectionMarker marker;
                    bool addToDict = false;
                
                    if(!dict.TryGetValue(junctionId, out marker))
                    {
                        marker = new DirectionMarker();
                        addToDict = true;
                    }

                    if (dir.Equals(SearchDirection.SEARCH_NEXT))
                    {
                        marker.searchNext = true;
                    }
                    if (dir.Equals(SearchDirection.SEARCH_PREV))
                    {
                        marker.searchPrev = true;
                    }
                    if(addToDict) {
                        dict.Add(junctionId, marker);
                    }
                }
            }
        }
    }
}
