using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using Client.UI.Proxy;

namespace Client.Resource
{
    /// <summary>
    /// This class maps the UIClasses to the corresponding model classes
    /// used for client server communication.
    /// </summary>
    static class ResourceMap
    {
        private static IDictionary<Type, ItemType> uiTypeToModelTypeMap;
        private static IDictionary<ItemType, Type> itemTypeToModelTypeMap;
        private static IDictionary<ItemType, Type> itemTypeToUITypeMap;

        
        //TODO: supported model class list (needed for building proxy classes goes here)

        static ResourceMap() 
        {
            uiTypeToModelTypeMap = new Dictionary<Type, ItemType>();
            itemTypeToModelTypeMap = new Dictionary<ItemType, Type>();
            itemTypeToUITypeMap = new Dictionary<ItemType, Type>();

            uiTypeToModelTypeMap.Add(typeof(UICollectionPoint), ItemType.CollectionPoint);
            itemTypeToModelTypeMap.Add(ItemType.CollectionPoint, typeof(CollectionPoint));
            itemTypeToUITypeMap.Add(ItemType.CollectionPoint, typeof(UICollectionPoint));

            uiTypeToModelTypeMap.Add(typeof(UICustomer), ItemType.Customer);
            itemTypeToModelTypeMap.Add(ItemType.Customer, typeof(Customer));
            itemTypeToUITypeMap.Add(ItemType.Customer, typeof(UICustomer));
        }

        /// <summary>
        /// </summary>
        /// <param name="uiType">The Type of the proxy class used in the User-Interface</param>
        /// <returns>The enum needed for client - server communication</returns>
        public static ItemType UITypeToItemType<T>() where T : UIItem 
        {
            ItemType t;
            uiTypeToModelTypeMap.TryGetValue(typeof(T), out t);
            return t;
        }

        /// <summary>
        /// </summary>
        /// <param name="itemType">The enum constant used for client - server communication</param>
        /// <returns>The Type of the model class returned from server</returns>
        public static Type ItemTypeToModelType(ItemType itemType)
        {
            Type t;
            itemTypeToModelTypeMap.TryGetValue(itemType, out t);
            return t;
        }

        /// <summary>
        /// </summary>
        /// <param name="itemType">The enum constant used for client - server communication</param>
        /// <returns>The Type of the proxy model class used in User-Interface</returns>
        public static Type ItemTypeToUIType(ItemType itemType)
        {
            Type t;
            itemTypeToUITypeMap.TryGetValue(itemType, out t);
            return t;
        }
    }
}
