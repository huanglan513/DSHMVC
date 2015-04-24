using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Entity;
using DSHOrder.Repository;

namespace DSHOrder.Service
{
    public class CodeTableService : ICodeTableService
    {
        IRepository repository = null;
        public CodeTableService()
        {
            repository = new CodeTableRepository();
        }
        #region ICodeTableService 成员

        public IList<CodeTable> GetCodeTablesByType(int type)
        {
            return repository.CreateQuery<CodeTable>().Where(p => p.CodeTypeID == type).ToList();
        }

        public IList<CodeTable> GetCodeTablesByTypes(int[] types)
        {
            return repository.CreateQuery<CodeTable>().Where(p => p.CodeTypeID.HasValue && types.Contains(p.CodeTypeID.Value)).ToList();
        }

        public CodeTable GetCodeTableByID(int id)
        {
            return repository.GetBy<CodeTable>(p => p.CodeID == id);
        }

        #endregion

        #region ICodeTableService 成员


        public CodeTable GetCodeTableByTypeAndValue(int CodeTypeID, string CodeValue)
        {
            return repository.CreateQuery<CodeTable>().Where(p => p.CodeTypeID == CodeTypeID && p.CodeValue == CodeValue).SingleOrDefault();
        }

        #endregion

        #region ICodeTableService 成员


        public IList<CodeTable> GetAll()
        {
            return repository.GetAll<CodeTable>();
        }

        #endregion
    }
}
