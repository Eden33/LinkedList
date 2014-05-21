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
        private static IDictionary<ItemType, Type> map = new Dictionary<ItemType, Type>();

        static ResourceMap()
        {
            map.Add(ItemType.Customer, typeof(Customer));
            map.Add(ItemType.CollectionPoint, typeof(CollectionPoint));
        }
        
        /// <summary>
        /// Returns the Type of the model class.
        /// </summary>
        /// <param name="itemType">The enum constant used for client - server communication</param>
        /// <returns>The Type of the model calss returned from server</returns>
        public static Type getModelType(ItemType itemType)
        {
            Type t;
            map.TryGetValue(itemType, out t);
            return t;
        }
    }
}
