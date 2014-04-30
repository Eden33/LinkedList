using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace ConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {
            using(ServiceHost host = new ServiceHost(typeof(Service.ResourceService)))
            {
                host.Open();
                Console.WriteLine("Service started. Press any key to stop it.");
                Console.ReadLine();
            }
        }
    }
}
