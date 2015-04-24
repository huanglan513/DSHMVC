using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSHOrder.Web.Extensions
{
public class DefaultListProvider : IListProvider
{
    private static Dictionary<string, IEnumerable<ListItem>> listItems = new Dictionary<string, IEnumerable<ListItem>>();
    static DefaultListProvider()
    {
        var items = new ListItem[]{
        new ListItem{ Text = "男", Value="M"},
        new ListItem{ Text = "女", Value="F"}};
        listItems.Add("Gender", items);

        items = new ListItem[]{            
        new ListItem{ Text = "人事部", Value="HR"}  , 
        new ListItem{ Text = "行政部", Value="AD"},
        new ListItem{ Text = "IT部", Value="IT"}};
        listItems.Add("Department", items);
       
    }
    public IEnumerable<ListItem> GetListItems(string listName)
    {
        IEnumerable<ListItem> items;
        if (listItems.TryGetValue(listName, out items))
        {
            return items;
        }
        return new ListItem[0];
    }
}
}