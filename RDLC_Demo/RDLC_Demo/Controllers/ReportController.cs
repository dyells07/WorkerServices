using AspNetCore.Reporting;
using Bytescout.BarCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RDLC_Demo.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace RDLC_Demo.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRepositoryVoucher _voucherRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(IWebHostEnvironment webHostEnvironment, IRepositoryVoucher voucherRepository)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _voucherRepository = voucherRepository ?? throw new ArgumentNullException(nameof(voucherRepository));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task<IActionResult> IndexAsync()
        {
            string mimeType = "application/pdf";
            int extension = 1;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Report1.rdlc");

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                 {"CName", "Example Company"},
  {"CAddress", "RatnaChowk, Pokhara"},
  {"VType", "JOURNAL VOUCHER"},
  {"CostCenter", "Main"},
  {"Date", "2080-8-23"},
  {"Vcode", "234"},
  {"PName", "Being Paid Amount to Mr. Anunnya Baral from Cheque No:39667472"},
  {"PBBy", "Kumar Gurung"}
            };

            var barcodeValue = "2345";
            Barcode barcode = new Barcode();
            barcode.Symbology = SymbologyType.Code128;
            barcode.Value = barcodeValue;
            barcode.Margins = new Margins(50, 50, 0, 0);
            Image barcodeImage = barcode.GetImage();

            using (MemoryStream stream = new MemoryStream())
            {
                barcodeImage.Save(stream, ImageFormat.Png);
                var barcodeBase64 = Convert.ToBase64String(stream.ToArray());
                parameters.Add("VBarcode", barcodeBase64);
            }

            var Voucher = await _voucherRepository.GetVoucherDetail();

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("Report", Voucher);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);

            return File(result.MainStream, mimeType);
        }
    }
}
