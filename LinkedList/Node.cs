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

        public Node()
        {
        }

        public Node(object o)
        {
            value = o;
        }

        public void InsertAtEnd(object o)
        {
            if (value == null)
                value = o;
            else
            {
                if (next == null)
                {
                    next = new Node(o);
                }
                else
                    next.InsertAtEnd(o);
            }
        }

        public bool Exists(object o)
        {
            if (value != null)
            {
                if (value.Equals(o))
                    return true;
                else
                {
                    if (next != null)
                        return next.Exists(o);
                    else return false;
                }
            }
            else return false;
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
                    value = 0;
                else
                {
                    if (next != null)
                    {
                        Node new_node = new Node(value);
                        value = o;
                        new_node.next = next;
                        next = new_node;
                    }
                    else
                    {
                        next = new Node(value);
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
