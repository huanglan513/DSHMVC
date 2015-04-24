using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Service.Interface
{
    public interface ISystemParamService
    {
        IList<Entity.SystemParam> GetAllSystemData();
        Entity.SystemParam GetSystemRecordByID(int id);
        Entity.SystemParam GetSystemRecordByName(string name);
        bool UpdateSystemValue(Entity.SystemParam sys);
    }
}
