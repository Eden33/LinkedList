using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    interface ISearchable
    {        
        Node FindNext(object value);
        Node FindPrev(object value);
    }

    public class Node : ISearchable
    {
        private object value = null;
        private ISearchable next = null;
        private ISearchable prev = null;

        public object Value 
        {
            get { return value; } 
        }

        public Node()
        {
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
        /// At the end of each Node-Path exatly one JunctionPoint can be added.
        /// </summary>
        /// <param name="j"></param>
        /// <returns>The JunctionPoint if added successfully, null if allready a JunctionPoint is added to the end of this Node-Path.</returns>
        public JunctionPoint InsertAtEnd(JunctionPoint j)
        {
            if (next == null)
            {
                next = j;
                return j;
            }
            else if (next is Node)
            {
                Node nextNode = (Node)next;
                return nextNode.InsertAtEnd(j);
            }

            // else path allready has a JunctionPoint set
            return null;
        }

        /// <summary>
        /// Search in Next-Direction, the value of current Node is checked too.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>The Node if found, otherwise null</returns>
        public Node FindNext(object o)
        {
            if (value != null)
            {
                if (value.Equals(o))
                    return this;
                else
                {
                    if (next != null)
                        return next.FindNext(o);
                    else return null;
                }
            }
            else if (next is JunctionPoint)
            {
                JunctionPoint nextJunction = (JunctionPoint) next;
                return nextJunction.FindNext(o);
            }
            else return null;
        }

        /// <summary>
        /// Search in Prev-Direction, the value of current Node is checked too.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>The Node if found, otherwise null</returns>
        public Node FindPrev(object o)
        {
            if (value != null)
            {
                if (value.Equals(o))
                    return this;
                else
                {
                    if (prev != null)
                        return prev.FindPrev(o);
                    else return null;
                }
            }
            else if (prev is JunctionPoint)
            {
                JunctionPoint prevJunction = (JunctionPoint) prev;
                return prevJunction.FindPrev(o);
            }
            else return null;
        }
    }

    public class JunctionPoint : ISearchable
    {
        private List<Node> nodes = new List<Node>();

        public Node FindNext(object value)
        {
            Node found = null;
            foreach (Node n in nodes)
            {
                found = n.FindNext(value);
                if (found != null)
                    return found;
            }
            return null;
        }

        public Node FindPrev(object value)
        {
            Node found = null;
            foreach (Node n in nodes)
            {
                found = n.FindPrev(value);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
