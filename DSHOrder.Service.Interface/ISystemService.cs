using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Service.Interface
{
    public interface ISystemService
    {
        IList<Entity.System> GetAllSystemData();
        Entity.System GetSystemRecordByID(int id);
        Entity.System GetSystemRecordByName(string name);
        bool UpdateSystemValue(Entity.System sys);
    }
}
