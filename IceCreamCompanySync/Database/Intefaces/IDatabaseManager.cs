using System.Data.SqlClient;

namespace IceCreamCompanySync.Database.Intefaces
{
    public interface IDatabaseManager
    {
        bool ExecuteNotQuery(string p_Command, List<SqlParameter> prms);
    }
}
