using Core.DataAccess.Data.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Data.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = "";

        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            return (T)Convert.ChangeType(sqlConnection.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
        }

        public void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            sqlConnection.Execute(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            return sqlConnection.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
