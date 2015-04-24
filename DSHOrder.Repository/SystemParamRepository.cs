using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;


namespace DSHOrder.Repository
{
    public class SystemParamRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntitySystemParam; }
        }

        public IList<Entity.SystemParam> GetAllSystemData()
        {
            var query = from q in this.CreateQuery<Entity.SystemParam>()
                        select q;
            return query.ToList<Entity.SystemParam>();
                        
        }

        public Entity.SystemParam GetSystemRecordByName(string name)
        {
            var query = from q in this.CreateQuery<Entity.SystemParam>()
                        where q.SystemName == name
                        select q;
            return query.SingleOrDefault<Entity.SystemParam>();
        }

        public Entity.SystemParam GetSystemRecordByID(int id)
        {
            var query = from q in this.CreateQuery<Entity.SystemParam>()
                        where q.SystemID == id
                        select q;
            return query.SingleOrDefault<Entity.SystemParam>();
        }
    }
}
