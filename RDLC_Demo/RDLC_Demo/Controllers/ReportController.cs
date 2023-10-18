using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RDLC_Demo.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using Bytescout.BarCode;

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
        }

        public async Task<IActionResult> IndexAsync()
        {
            string mimeType = "application/pdf";
            int extension = 1;
            var reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Report1.rdlc");

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
            var barcodeImage = GenerateBarcodeImage(barcodeValue);

            // Convert the barcode image to Base64 string
            string barcodeBase64;
            using (var stream = new MemoryStream())
            {
                barcodeImage.Save(stream, ImageFormat.Png);
                barcodeBase64 = Convert.ToBase64String(stream.ToArray());
            }

            parameters.Add("VBarcode", barcodeBase64);

            var voucherData = await _voucherRepository.GetVoucherDetail();

            var reportResult = GenerateReport(reportPath, voucherData, parameters, extension, mimeType);

            return new FileContentResult(reportResult, mimeType)
            {
                FileDownloadName = "Report.pdf"
            };
        }

        private Image GenerateBarcodeImage(string value)
        {
            var barcode = new Barcode
            {
                Symbology = BarcodeSymbology.Code128,
                Value = value,
                Margins = new Margins(50, 50, 0, 0)
            };

            return barcode.GetImage();
        }

        private byte[] GenerateReport(string reportPath, object dataSource, Dictionary<string, string> parameters, int extension, string mimeType)
        {
            var reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Local, LocalReportPath = reportPath };

            // Set the report data source
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Report", dataSource));

            // Set the report parameters
            foreach (var parameter in parameters)
            {
                reportViewer.LocalReport.SetParameters(new ReportParameter(parameter.Key, parameter.Value));
            }

            var reportBytes = reportViewer.LocalReport.Render(extension.ToString(), null, out var fileExtension, out var encoding, out var mimeType, out var warnings);

            return reportBytes;
        }
    }
}