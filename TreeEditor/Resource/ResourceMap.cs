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
        private static IDictionary<Type, ItemType> map;

        static ResourceMap() 
        {
            map = new Dictionary<Type, ItemType>();
            map.Add(typeof(UICollectionPoint),  ItemType.CollectionPoint);
            map.Add(typeof(UIClient),           ItemType.Client);
        }

        /// <summary>
        /// Returns the type needed for client - server communication.
        /// </summary>
        /// <param name="uiType">The Type of the proxy class used in the User-Interface</param>
        /// <returns>The enum constant needed for client - server communication</returns>
        public static ItemType GetModelType(Type uiType)
        {
            ItemType t;
            map.TryGetValue(uiType, out t);
            return t;
        }
    }
}
