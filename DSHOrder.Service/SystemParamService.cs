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
    public class SystemParamService : ISystemParamService
    {
        SystemParamRepository sysRepos = null;

        public SystemParamService()
        {
            sysRepos = new SystemParamRepository();
        }

        public IList<Entity.SystemParam> GetAllSystemData()
        {
            return sysRepos.GetAllSystemData();
        }

        public Entity.SystemParam GetSystemRecordByID(int id)
        {
            return sysRepos.GetSystemRecordByID(id);
        }

        public Entity.SystemParam GetSystemRecordByName(string name)
        {
            return sysRepos.GetSystemRecordByName(name);
        }

        public bool UpdateSystemValue(Entity.SystemParam sys)
        {
            Entity.SystemParam modified = sysRepos.Update<Entity.SystemParam>(sys, false);
            return modified != null;
        }
    }
}
