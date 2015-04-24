using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Service
{
    public class SystemService : ISystemService
    {
        SystemRepository sysRepos = null;

        public SystemService()
        {
            sysRepos = new SystemRepository();
        }

        public IList<Entity.System> GetAllSystemData()
        {
            return sysRepos.GetAllSystemData();
        }

        public Entity.System GetSystemRecordByID(int id)
        {
            return sysRepos.GetSystemRecordByID(id);
        }

        public Entity.System GetSystemRecordByName(string name)
        {
            return sysRepos.GetSystemRecordByName(name);
        }

        public bool UpdateSystemValue(Entity.System sys)
        {
            Entity.System modified = sysRepos.Update<Entity.System>(sys, false);
            return modified != null;
        }
    }
}
