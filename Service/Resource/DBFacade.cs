using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    public class DBFacade : IData
    {
        /// <summary>
        /// Contains all data items available in this facade data source
        /// </summary>
        private ResourceCache dataSource = new ResourceCache();

        /// <summary>
        /// Monitor object to block concurrent access during update the item cache
        /// </summary>
        private object cacheBlockUpdatesMonitor = new object();

        public DBFacade()
        {
            GenerateRecordData(10, 50, 3);
            //GenerateStaticRecordData();
        }

        #region IDate get and block cached resources

        public ResourceCache Cache 
        {
            get { return this.dataSource;  }
        }

        public object CacheBlockUpdatesMonitor
        {
            get { return cacheBlockUpdatesMonitor; }
        }

        #endregion

        #region IData get available resources

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

        #region helper methods to generate data items

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
                        try
                        {
                            c = customers.ElementAt(random.Next(1, countCustomers));
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            Console.WriteLine(e.Message);
                        }
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
            dataSource.CacheAllItems<Customer>(customers);
          
            #endregion

            Console.WriteLine("Data with random dependencies created .... CP count: {0} Customer count: {1}", countCP, countCustomers);

        }
    
        private void GenerateStaticRecordData()
        {
            for(int i = 1; i <= 6; i++)
            {
                Customer c = new Customer(i);
                c.FirstName = "First Name " + i;
                c.LastName = "Last Name " + i;
                c.Address = "Adress " + i;
                dataSource.CacheItem<Customer>(c);

            }
            CollectionPoint cp1 = new CollectionPoint(1);
            cp1.Description = "This is CP 1";
            cp1.Address = "Adress of CP 1";
            cp1.Customers.Add(dataSource.GetItem<Customer>(1));
            cp1.Customers.Add(dataSource.GetItem<Customer>(2));
            cp1.Customers.Add(dataSource.GetItem<Customer>(3));

            CollectionPoint cp2 = new CollectionPoint(2);
            cp2.Description = "This is CP 2";
            cp2.Address = "Adress of CP 2";
            cp2.Customers.Add(dataSource.GetItem<Customer>(2));
            cp2.Customers.Add(dataSource.GetItem<Customer>(3));
            cp2.Customers.Add(dataSource.GetItem<Customer>(4));

            CollectionPoint cp3 = new CollectionPoint(3);
            cp3.Description = "This is CP 3";
            cp3.Address = "Adress of CP 3";
            cp3.Customers.Add(dataSource.GetItem<Customer>(2));

            CollectionPoint cp4 = new CollectionPoint(4);
            cp4.Description = "This is CP 4";
            cp4.Address = "Adress of CP 4";
            cp4.Customers.Add(dataSource.GetItem<Customer>(5));
            cp4.Customers.Add(dataSource.GetItem<Customer>(6));

            dataSource.CacheItem<CollectionPoint>(cp1);
            dataSource.CacheItem<CollectionPoint>(cp2);
            dataSource.CacheItem<CollectionPoint>(cp3);
            dataSource.CacheItem<CollectionPoint>(cp4);
        }

        #endregion
    }
}