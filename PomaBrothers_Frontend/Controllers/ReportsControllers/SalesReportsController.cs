using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models.DTOModels;
using PomaBrothers_Frontend.Reports.Implementation.SaleReports;
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace PomaBrothers_Frontend.Controllers.ReportsControllers
{
    public class SalesReportsController : Controller
    {
        private HttpClient _httpClient = new();
        private readonly IWebHostEnvironment _host;

        public SalesReportsController(IWebHostEnvironment host)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5164/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _host = host;
        }

        public async Task<ActionResult> GeneratedReportPurchasesCustomer(int customerId)
        {
            var purchases = await GetPurchasesByCustomer(customerId);
            if(purchases != null)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    PurchasesCustomerReport customerReport = new(_host, purchases);
                    customerReport.TitleReport = "reporte\nventas";
                    customerReport.CreateDocument().GeneratePdf(memoryStream);
                    memoryStream.Position = 0;
                    return File(memoryStream.ToArray(), "application/pdf");
                }
                catch
                {
                    throw;
                }
            }
            return NotFound();
        }

        public async Task<PurchasesCustomerDTO> GetPurchasesByCustomer(int customerId)
        {
            HttpResponseMessage request = await _httpClient.GetAsync($"SalesReports/GetPurchasesCustomerReport/{customerId}");
            if (request.IsSuccessStatusCode)
            {
                string json = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<PurchasesCustomerDTO>(json);
            }
            return null!;
        }

        public async Task<ActionResult> GeneratedReportSalesByDateRange(DateTime startDate, DateTime finishDate)
        {
            var salesbyRange = await GetPurchasedDTOAsync(startDate, finishDate);
            if(salesbyRange != null)
            {
                var total = salesbyRange.Sum(sale => sale.Total);
                try
                {
                    var memoryStream = new MemoryStream();
                    SalesByDateRangeReport salesByDateRange = new(startDate, finishDate, total, salesbyRange, _host);
                    salesByDateRange.TitleReport = "reporte\nventas";
                    salesByDateRange.CreateDocument().GeneratePdf(memoryStream);
                    memoryStream.Position = 0;
                    return File(memoryStream.ToArray(), "application/pdf");
                }
                catch
                {
                    throw;
                }
            }
            return NotFound("There were no sales in this date range");
        }

        public async Task<List<SaleDTO>> GetPurchasedDTOAsync(DateTime startDate, DateTime finishDate)
        {
            string formattedStartDate = Uri.EscapeDataString(startDate.ToString("MM/dd/yyyy"));
            string formattedFinishDate = Uri.EscapeDataString(finishDate.ToString("MM/dd/yyyy"));
            string url = $"SalesReports/GetSalesRangeReport?startDate={formattedStartDate}&endDate={formattedFinishDate}";
            HttpResponseMessage request = await _httpClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                string json = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<SaleDTO>>(json);
            }
            return null!;
        }
    }
}
