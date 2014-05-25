using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Model.Lock;
using Service.Transaction;
using Service.Resource;

namespace Service.Lock.LockStrategy
{
    public class LockCollectionPointStrategy : ILockStrategy
    {
        private TransactionManager tm;

        public LockCollectionPointStrategy(TransactionManager tm)
        {
            this.tm = tm;
        }

        public LockBatch GetItemsToLock(int id)
        {
            Console.WriteLine("Client sent lock request for CollectionPoint with id: {0}", id);

            ResourceCache currentItems = tm.Cache;
            List<CollectionPoint> allCollectionPoints = currentItems.GetAllItems<CollectionPoint>();
            List<Customer> allCustomers = currentItems.GetAllItems<Customer>();

  
            LockItem cpLockItem = new LockItem();
            cpLockItem.ItemType = ItemType.CollectionPoint;
            cpLockItem.IDsToLock = new List<int>();

            LockItem customerLockItem = new LockItem();
            customerLockItem.ItemType = ItemType.Customer;
            customerLockItem.IDsToLock = new List<int>();

            LockBatch batch = new LockBatch();

            CollectionPoint targetItem = currentItems.GetItem<CollectionPoint>(id);
            if(targetItem != null)
            {
                batch.ItemsToLock = new List<LockItem>(new LockItem[] { cpLockItem, customerLockItem });

                cpLockItem.IDsToLock.Add(id); //root id of the lock request
                
                // get the customers 
                List<Customer> targetCustomers = targetItem.Customers;

                // add the customers of target item first
                foreach(Customer currentCustomer in targetCustomers)
                {
                    customerLockItem.IDsToLock.Add(currentCustomer.Id);
                }

                // go over the other CollectionPoints and get the dependencies
                int oldCustomerCount = -1;

                //TODO: do we need to observe oldCollectionPoint count too?

                do
                {
                    oldCustomerCount = customerLockItem.IDsToLock.Count;

                    foreach(CollectionPoint currentCollectionPoint in allCollectionPoints)
                    {
                        if(cpLockItem.IDsToLock.Contains(currentCollectionPoint.Id) == false)
                        {
                            bool dependencyExists = false;
                            List<int> customerIdsOfCurrentCP = new List<int>();
                            //check if this collection point contains at least one customer with
                            //which is allready marked as locked
                            foreach(Customer currentCustomer in currentCollectionPoint.Customers)
                            {
                                customerIdsOfCurrentCP.Add(currentCustomer.Id);
                                if(customerLockItem.IDsToLock.Contains(currentCustomer.Id)) 
                                {
                                    dependencyExists = true;
                                }
                            }
                            if(dependencyExists == true)
                            {
                                // if new dependency found we need to add the CollectionPoint
                                // and the customer not allready marked to be locked
                                foreach(int customerId in customerIdsOfCurrentCP)
                                {
                                    if(customerLockItem.IDsToLock.Contains(customerId) == false)
                                    {
                                        customerLockItem.IDsToLock.Add(customerId);
                                    }
                                }
                                cpLockItem.IDsToLock.Add(currentCollectionPoint.Id);
                            }
                        }
                    }
                } while (oldCustomerCount.Equals(customerLockItem.IDsToLock.Count) == false);
            }
            else
            {
                Console.WriteLine("CollectionPoint root item: {0} isn't cached by the data source!", id);
                batch.ItemsToLock = null;
            }

            return batch;
        }
    }
}
