using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            Node n = new Node();
            n.InsertAtEnd(5);
            n.InsertAtEnd(10);
            n.InsertAtEnd(15);
            Console.WriteLine(n.NodeToString());
            n.ChangeAtPosition(1, 11);
            Console.WriteLine(n.NodeToString());
            n.insertAt(0, 10);
            Console.WriteLine(n.NodeToString());
            
            Console.ReadKey();
        }
    }
}
