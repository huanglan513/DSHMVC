using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSHOrder.Web.Extensions
{
    public interface IListProvider
    {
        IEnumerable<ListItem> GetListItems(string listName);
    }
}
