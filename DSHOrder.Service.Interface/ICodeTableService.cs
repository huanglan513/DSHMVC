using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface ICodeTableService
    {
        IList<CodeTable> GetCodeTablesByType(int type);

        IList<CodeTable> GetCodeTablesByTypes(int[] types);

        CodeTable GetCodeTableByID(int id);

        CodeTable GetCodeTableByTypeAndValue(int CodeTypeID, string CodeValue);

        IList<CodeTable> GetAll();
    }
}
