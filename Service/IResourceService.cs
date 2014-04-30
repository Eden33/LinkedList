using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Model.Data;

namespace Service
{
    [ServiceContract]
    public interface IResourceService
    {
        [OperationContract]
        bool TryLock(int id, Type type);
        
        [OperationContract]
        CollectionPoint GetCollectionPoint(int id);
        
        [OperationContract]
        CollectionVat GetCollectionVat(int id);
    }
}
