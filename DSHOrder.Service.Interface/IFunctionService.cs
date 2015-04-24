using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IFunctionService
    {
        int DeleteFunction(Function func);

        IList<Function> GetAllFunctions();

        Function GetByID(int id);

        Function AddFunction(Function function);

        Function UpdateFunction(Function function);

        IList<Function> GetAllMenus(int roleID);

        Function GetByName(string FunctionName);
    }
}
