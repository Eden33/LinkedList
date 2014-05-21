using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Data;

namespace Service.Resource
{
    interface IData
    {
        T GetSingleItem<T>(int id) where T : Item;

        List<T> GetAllItems<T>() where T : Item;
    }
}
