using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Model.Data;

namespace Service
{
    public class ResourceService : IResourceService
    {
        public bool TryLock(int id, Type type)
        {
            return true;
        }

        public Model.Data.CollectionPoint GetCollectionPoint(int id)
        {
            return ResourceManager.getCollectionPoint(id);
        }

        public Model.Data.CollectionVat GetCollectionVat(int id)
        {
            return ResourceManager.getCollectionVat(id);
        }
    }
}
