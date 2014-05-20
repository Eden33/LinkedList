using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;
using TreeEditor.UI.Proxy;

namespace TreeEditor.Resource
{
    /// <summary>
    /// This class maps the UIClasses to the corresponding model classes
    /// used for client server communication.
    /// </summary>
    static class ResourceMap
    {
        private static IDictionary<Type, ItemType> modelMap;
        private static IDictionary<ItemType, Type> clientMap;

        static ResourceMap() 
        {
            modelMap = new Dictionary<Type, ItemType>();
            clientMap = new Dictionary<ItemType, Type>();

            modelMap.Add(typeof(UICollectionPoint), ItemType.CollectionPoint);
            clientMap.Add(ItemType.CollectionPoint, typeof(CollectionPoint));

            modelMap.Add(typeof(UIClient), ItemType.Client);
            clientMap.Add(ItemType.Client, typeof(Client));
        }

        /// <summary>
        /// Returns the enum needed needed for client - server communication
        /// </summary>
        /// <param name="uiType">The Type of the proxy class used in the User-Interface</param>
        /// <returns>The enum needed for client - server communication</returns>
        public static ItemType GetItemType<T>() where T : UIItem 
        {
            ItemType t;
            modelMap.TryGetValue(typeof(T), out t);
            return t;
        }

        /// <summary>
        /// Returns the Type of the model class.
        /// </summary>
        /// <param name="itemType">The enum constant used for client - server communication</param>
        /// <returns>The Type of the model class returned from server</returns>
        public static Type getModelType(ItemType itemType)
        {
            Type t;
            clientMap.TryGetValue(itemType, out t);
            return t;
        }
    }
}
