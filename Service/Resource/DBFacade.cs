using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    class DBFacade : IData
    {
        /// <summary>
        /// Contains all data elements available in this facade data source
        /// </summary>
        private ResourceCache dataSource = new ResourceCache();

        /// <summary>
        /// Contains all data elements that have been requested allready
        /// </summary>
        private ResourceCache cache = new ResourceCache();

        public DBFacade()
        {
            GenerateRecordData(10, 50, 3);
        }

        #region IData

        public T GetSingleItem<T>(int id) where T : Item
        {
            T item = dataSource.GetItem<T>(id);
            return item;
        }

        public List<T> GetAllItems<T>() where T : Item
        {
            List<T> items = dataSource.GetAllItems<T>();
            return items;
        }

        #endregion

        /// <summary>
        /// Helper to generate a predefined amount of data objects for the data source facade 
        /// and set up random dependencies between them as specified by the abstract data model.
        /// </summary>
        /// <param name="countCP"></param>
        /// <param name="countCustomers"></param>
        /// <param name="maxCustomerCPCount"></param>
        private void GenerateRecordData(int countCP, int countCustomers, int maxCustomerCPCount)
        {
            Console.WriteLine("Generate data with random dependencies ....");

            Random random = new Random();

            #region generate Customers

            IList<Customer> customers = new List<Customer>();
            Customer c = null;
            for(int i = 1; i <= countCustomers; i++)
            {
                c = new Customer(i);
                c.FirstName = "First Name " + i;
                c.LastName = "Last Name " + i;
                c.Address = "Address " + i;
                customers.Add(c);
            }
            dataSource.CacheAllItems<Customer>(customers);
            #endregion

            #region generate CPs and set up dependencies

            IList<CollectionPoint> cps = new List<CollectionPoint>();
            CollectionPoint cp = null;
            for (int i = 1; i <= countCP; i++)
            {
                cp = new CollectionPoint(i);
                cp.Description = "This is CollectionPoint Nr: " + i;
                cps.Add(cp);

                //set up random dependencies
                int tmpRand = random.Next(0, (maxCustomerCPCount + 1));
                if(tmpRand != 0)
                {
                    for(int id = 1; id <= tmpRand; id++)
                    {
                        c = dataSource.GetItem<Customer>(id);
                        if(c != null)
                        {
                            cp.Customers.Add(c);
                        }
                        else
                        {
                            Console.WriteLine("Customer with id: {0} is null!", id);
                        }
                    }
                }
            }
            dataSource.CacheAllItems<CollectionPoint>(cps);
          
            #endregion

            Console.WriteLine("Data with random dependencies created .... CP count: {0} Customer count: {1}", countCP, countCustomers);

        }
    }
}