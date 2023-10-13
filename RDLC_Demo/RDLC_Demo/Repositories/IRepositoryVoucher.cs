using RDLC_Demo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDLC_Demo.Repositories
{
    public interface IRepositoryVoucher
    {
        Task<IEnumerable<VoucherDetail>> GetVoucherDetail();
    }
}
