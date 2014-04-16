using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class Node
    {
        private object value = null;
        private Node next = null;
        private Node prev = null;

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
        /// Inserts the value at the end and returns the containing Node.
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
            else
            {
                if (next == null)
                {
                    next = new Node(o);
                    next.prev = this;
                    return next;
                }
                else
                    return next.InsertAtEnd(o);
            }
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
            else return null;
        }

        public String NodeToString()
        {
            if (value == null)
                return "empty";
            else
            {
                if (next == null)
                    return value.ToString() + " - empty";
                else return value.ToString() + " - " + next.NodeToString();
            }
        }

        public void ChangeAtPosition(int index, object onew)
        {
            if(index == 0)
            {
                value = onew;
            }
            else
            {
                if (next != null)
                    next.ChangeAtPosition(--index, onew);
            }
        }

        public void insertAt(int index, object o)
        {
            if (index == 0)
            {
                if (value == null)
                    value = o;
                else
                {
                    if (next != null)
                    {
                        Node new_node = new Node(value);
                        value = o;
                        new_node.next = next;
                        new_node.prev = this;
                        next = new_node;
                    }
                    else
                    {
                        next = new Node(value);
                        next.prev = this;
                        value = o;
                    }
                }
            }
            else
            {
                if (next != null)
                    next.insertAt(--index, o);
            }
        }
    }
}
