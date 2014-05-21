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
        private ResourceCache cache = new ResourceCache();

        public DBFacade()
        {
            GenerateRandomRecordData(50, 5000, 30);
        }

        #region IData

        public T GetSingleItem<T>(int id) where T : Item
        {
            throw new NotImplementedException();
        }

        public List<T> GetAllItems<T>() where T : Item
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Helper to generate random record data and set up random dependencies
        /// </summary>
        /// <param name="countCP"></param>
        /// <param name="countCustomers"></param>
        /// <param name="customerCPCount"></param>
        private void GenerateRandomRecordData(int countCP, int countCustomers, int maxCustomerCPCount)
        {
            // TODO: implement me
        }
    }
}