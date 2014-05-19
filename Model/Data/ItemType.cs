using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{

    /// <summary>
    /// As in the book "Programming WCF services" stated (page 156) - WCF is not 
    /// made to pass generic type information between client and server
    /// 
    /// This class is used in combination with inheritance to bypass this limitiation.
    /// Therefore we need a mapping on client as well as on server side.
    /// </summary>
    [DataContract(Namespace= "http://itm4.gopp/resources/itemType")]
    public enum ItemType
    {
        [EnumMember]
        Client,
        [EnumMember]
        CollectionPoint
    }
}
