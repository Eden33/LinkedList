using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    // TODO: this class can be "merged" with RescourceMap.cs of the client application

    class ResourceMap
    {
        private static Dictionary<ItemType, Type> itemTypeToModelTypeMap = new Dictionary<ItemType, Type>();
        private static Dictionary<Type, ItemType> modelTypeToItemTypeMap = new Dictionary<Type, ItemType>();

        static ResourceMap()
        {
            itemTypeToModelTypeMap.Add(ItemType.Customer, typeof(Customer));
            modelTypeToItemTypeMap.Add(typeof(Customer), ItemType.Customer);

            itemTypeToModelTypeMap.Add(ItemType.CollectionPoint, typeof(CollectionPoint));
            modelTypeToItemTypeMap.Add(typeof(CollectionPoint), ItemType.CollectionPoint);
        }
        
        /// <summary>
        /// Returns the Type of the model class.
        /// </summary>
        /// <param name="itemType">The enum constant used for client - server communication</param>
        /// <returns>The Type of the model class returned from server to client</returns>
        public static Type ItemTypeToModelType(ItemType itemType)
        {
            Type t;
            itemTypeToModelTypeMap.TryGetValue(itemType, out t);
            return t;
        }

        public static ItemType ModelTypeToItemType<T>() where T : Item
        {
            ItemType t;
            modelTypeToItemTypeMap.TryGetValue(typeof(T), out t);
            return t;
        }
    }
}
