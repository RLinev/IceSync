using IceCreamCompanySync.HttpHandler.Models;
using System.Collections;
using System.Data;
using System.Reflection;

namespace IceCreamCompanySync
{
   

    public static class Utils
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            var item = source.First();
            var properties = new List<string>();

            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                properties.Add(property.Name);
            }

            var table = new DataTable();
            foreach (var prop in properties)
            {
                table.Columns.Add(prop);
            }

            foreach (var wf in source)
            {
                var wfValues = new ArrayList();
                foreach (var prop in properties)
                {
                    wfValues.Add(wf.GetType().GetProperty(prop).GetValue(wf));
                }
                table.Rows.Add(wfValues);
            }
            return table;
        }
    }
}
