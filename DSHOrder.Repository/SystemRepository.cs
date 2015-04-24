using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;


namespace DSHOrder.Repository
{
    public class SystemRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntitySystem; }
        }

        public IList<Entity.System> GetAllSystemData()
        {
            var query = from q in this.CreateQuery<Entity.System>()
                        select q;
            return query.ToList<Entity.System>();
                        
        }

        public Entity.System GetSystemRecordByName(string name)
        {
            var query = from q in this.CreateQuery<Entity.System>()
                        where q.SystemName == name
                        select q;
            return query.SingleOrDefault<Entity.System>();
        }

        public Entity.System GetSystemRecordByID(int id)
        {
            var query = from q in this.CreateQuery<Entity.System>()
                        where q.SystemID == id
                        select q;
            return query.SingleOrDefault<Entity.System>();
        }
    }
}
