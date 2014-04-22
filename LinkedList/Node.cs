using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedList
{
    interface ISearchable
    {        
        Node FindNext(object value, int searchId);
        Node FindPrev(object value, int searchId);
        Node Find(object value, int searchId);
    }

    public class Node : ISearchable
    {
        private object value = null;
        private ISearchable next = null;
        private ISearchable prev = null;
        public static readonly int ROOT_ID = 1;

        public object Value 
        {
            get { return value; } 
        }

        public Node(object o)
        {
            value = o;
        }

        /// <summary>
        /// Inserts the value at the end of this Node-Path and returns the containing Node.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>The Node containing o</returns>
        public Node InsertAtEnd(object o)
        {
            if (value == null) 
            {
                value = o;
                return this;
            }

            if (next == null)
            {
                Node nextNode = new Node(o);
                nextNode.prev = this;
                next = (ISearchable) nextNode;
                return nextNode;
            }
            else if(next is Node)
            {
                Node nextNode = (Node) next;
                return nextNode.InsertAtEnd(o);
            }

            //else next is a JunctionPoint, insert it bevor
            Node newNode = new Node(o);
            newNode.prev = this;
            newNode.next = next;
            return newNode;
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
        /// Search in Next-Direction, the value of current Node is checked too.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>The Node if found, otherwise null</returns>
        public Node FindNext(object o, int searchId)
        {
            if (value.Equals(o))
                return this;

            if(next is Node) 
            {
                return next.FindNext(o, searchId);          
            }
            else if (next is JunctionPoint)
            {
                JunctionPoint nextJunction = (JunctionPoint) next;
                return nextJunction.Find(o, searchId);
            }
            else return null;
        }

        /// <summary>
        /// Search in Prev-Direction, the value of current Node is checked too.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>The Node if found, otherwise null</returns>
        public Node FindPrev(object o, int searchId)
        {
            if (value.Equals(o))
                return this;

            if(prev is Node) 
            {
                return prev.FindPrev(o, searchId);            
            }
            else if (prev is JunctionPoint)
            {
                JunctionPoint prevJunction = (JunctionPoint) prev;
                return prevJunction.Find(o, searchId);
            }
            else return null;
        }

        public Node Find(object o, int searchId)
        {
            Node found = FindNext(o, searchId);
            if (found == null)
            {
                found = FindPrev(o, searchId);
            }
            return found;
        }

        #endregion
    }

    public class JunctionPoint : ISearchable
    {
        private List<Node> nextPath = new List<Node>();
        private List<Node> prevPath = new List<Node>();
        public static readonly int ROOT_ID = 1;
        private int id;

        public int Id {
            get { return id; }
        }

        public JunctionPoint(int id)
        {
            this.id = id;
        }

        public Node FindNext(object value, int searchId)
        {
            if (SearchBuffer.JunctionVisitedAllready(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_NEXT))
            {
                return null;
            }
            SearchBuffer.MarkJunctionVisited(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_NEXT);
            //Console.WriteLine("Search reached Junction " + this.id + " in next direction.");

            Node found = null;
            foreach (Node n in nextPath)
            {
                found = n.FindNext(value, searchId);
                if (found != null)
                    return found;
            }
            return null;
        }

        public Node FindPrev(object value, int searchId)
        {
            if (SearchBuffer.JunctionVisitedAllready(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_PREV))
            {
                return null;
            }
            SearchBuffer.MarkJunctionVisited(searchId, this.id, SearchBuffer.SearchDirection.SEARCH_PREV);
            //Console.WriteLine("Search reached Junction " + this.id + " in prev direction.");

            Node found = null;
            foreach (Node n in prevPath)
            {
                found = n.FindPrev(value, searchId);
                if (found != null)
                    return found;
            }
            return null;
        }

        public Node Find(object value, int searchId)
        {
            Node found = null;
            found = FindNext(value, searchId);
            if (found == null)
            {
                found = FindPrev(value, searchId);
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
