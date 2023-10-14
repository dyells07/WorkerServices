
using RDLC_Demo.Models;
using Microsoft.Extensions.Options;
using RDLC_Demo.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace RDLC_Demo.Models
{
    public class VoucherRepository : IRepositoryVoucher
    {
        private readonly string _connectionString;

        public VoucherRepository(IOptions<AppDbConnection> config)
        {
            _connectionString = config.Value.ConnectionString;
        }

        public async Task<IEnumerable<VoucherDetail>> GetVoucherDetail()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                //string procedureName = "GetVoucherDetailsWithSameVoucherName";
                //return await db.QueryAsync<VoucherDetail>(procedureName, commandType: CommandType.StoredProcedure);

                string query = "SELECT Title, Remarks, DrAmount, CrAmount,VoucherName FROM VoucherDetail";
                return await db.QueryAsync<VoucherDetail>(query, commandType: CommandType.Text);
            }
        }
    }
}
