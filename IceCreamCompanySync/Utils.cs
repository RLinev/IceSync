using IceCreamCompanySync.HttpHandler.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Reflection;

namespace IceCreamCompanySync
{
   

    public static class Utils
    {
        public static DataTable ToDataWorkflowTable(this IEnumerable<WorkflowModel> source)
        {
            var result = new DataTable();
            result.Columns.Add("WorkflowID");
            result.Columns.Add("WorkflowName");
            result.Columns.Add("IsActive");
            result.Columns.Add("IsRunning");
            result.Columns.Add("MultiExecBehavior");

            foreach (var wf in source)
            {
                result.Rows.Add(wf.WorkflowID, wf.WorkflowName, wf.IsActive, wf.IsRunning, wf.MultiExecBehavior);
            }

            return result;
        }
    }
}
