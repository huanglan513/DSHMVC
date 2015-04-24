using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class FunctionRepository:ObjectRepository
    {

        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityFunction; }
        }

        public Function GetByName(string FunctionName)
        {
            var query = from r in this.CreateQuery<Function>()
                        where r.FunctionName == FunctionName
                        select r;

            return query.FirstOrDefault();
        }
    }
}
