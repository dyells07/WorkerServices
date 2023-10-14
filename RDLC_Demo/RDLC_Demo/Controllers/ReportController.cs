using AspNetCore.Reporting;
using RDLC_Demo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RDLC_Demo.Repositories;


namespace RDLC_Demo.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRepositoryVoucher _voucherRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment, IRepositoryVoucher voucherRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _voucherRepository = voucherRepository;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task<IActionResult> IndexAsync()
        {
            string mimeType = "application/pdf";
            int extension = 1;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Report1.rdlc");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CName", "Example Company");
            parameters.Add("CAddress", "RatnaChowk, Pokhara");
            parameters.Add("VType", "JOURNAL VOUCHER");
            parameters.Add("CostCenter", "Main");
            parameters.Add("Date", "2080-8-23");
            parameters.Add("PName", "Being Paid Amount to Mr. Anunnya Baral from Cheque No:39667472");
            parameters.Add("PBBy", "Kumar Gurung");
            var Voucher = await _voucherRepository.GetVoucherDetail();




            //LocalReport localReport = new LocalReport(path);
            //object value = localReport.AddDataSource("Report", Voucher);
            //var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("Report", Voucher);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);


            return File(result.MainStream, mimeType);
        }
        public async Task<IActionResult> GeneralVoucherAsync()
        {
            string mimeType = "application/pdf";
            int extension = 1;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "GeneralVoucher.rdlc");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CName", "Example Company");
            parameters.Add("CAddress", "Simalchour-8, Pokhara");
            parameters.Add("VType", "JOURNAL VOUCHER");
            parameters.Add("CostCenter", "Main");
            parameters.Add("Date", "2080-8-23");
            parameters.Add("PName", "Being Paid Amount to Mr. Anunnya Baral from Cheque No:39667472");
            parameters.Add("PBBy", "Kumar Gurung");
            var Voucher = await _voucherRepository.GetVoucherDetail();




            //LocalReport localReport = new LocalReport(path);
            //object value = localReport.AddDataSource("Report", Voucher);
            //var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("Report", Voucher);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);


            return File(result.MainStream, mimeType);
        }






    }
}