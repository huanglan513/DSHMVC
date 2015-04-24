using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class FunctionService:IFunctionService
    {
        FunctionRepository repository = null;
        public FunctionService()
        {
            repository = new FunctionRepository();
        }

        #region IFunctionService 成员

        public int DeleteFunction(Function func)
        {
            return repository.Delete<Function>(func);
        }

        public IList<Function> GetAllFunctions()
        {
            return repository.GetAll<Function>();
        }

        public IList<Function> GetAllMenus(int roleID)
        {
            var query = from q in repository.CreateQuery<Function>()
                        join p in repository.CreateQuery<Privilege>() on q.FunctionID equals p.FunctionID
                        where roleID == p.RoleID && q.ParentID != 999
                        select q;
            return query.ToList<Function>();
        }

        public Function GetByID(int id)
        {
            return repository.GetBy<Function>(q => q.FunctionID==id);
        }

        public Function AddFunction(Function function)
        {
            return repository.Add<Function>(function);
        }

        public Function UpdateFunction(Function function)
        {
            return repository.Update<Function>(function);
        }

        #endregion

        #region IFunctionService 成员


        public Function GetByName(string FunctionName)
        {
            return repository.GetByName(FunctionName);
        }

        #endregion
    }
}
