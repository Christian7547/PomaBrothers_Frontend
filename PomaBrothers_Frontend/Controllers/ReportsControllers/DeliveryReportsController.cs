using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PomaBrothers_Frontend.Models;
using PomaBrothers_Frontend.Reports.Implementation.DeliveryReports;
using QuestPDF.Fluent;
using PomaBrothers_Frontend.Models.DTOModels;

namespace PomaBrothers_Frontend.Controllers.ReportsControllers
{
    public class DeliveryReportsController : Controller
    {
        private HttpClient _httpClient = new();
        private readonly IWebHostEnvironment _host;

        public DeliveryReportsController(IWebHostEnvironment host)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5164/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _host = host;

        }

        public async Task<ActionResult> GeneratedReportByDateRange(DateTime startDate, DateTime finishDate)
        {
            var itemsByDateRange = await GetOrdersByDateRangeReport(startDate, finishDate);
            var total = itemsByDateRange.Sum(purchasePrice => purchasePrice.PurchasePrice);
            if (itemsByDateRange != null)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    OrdersByDateRangeReport report = new(startDate, finishDate, total, itemsByDateRange, _host);
                    report.TitleReport = "reporte de pedidos";
                    report.CreateDocument().GeneratePdf(memoryStream);
                    memoryStream.Position = 0;
                    return File(memoryStream, "application/pdf", $"ReportePedidos_{startDate:yyyyMMdd}-{finishDate:yyyyMMdd}.pdf");
                }
                catch
                {
                    throw;
                }
            }
            return NotFound("No hay pedidos realizados en ese rango de fechas");
        }


        public async Task<List<DeliveryDetail>> GetOrdersByDateRangeReport(DateTime startDate, DateTime finishDate)
        {
            string url = $"DeliveryReports/GetOrdersByDateRangeReport?startDate={startDate}&endDate={finishDate}";
            HttpResponseMessage request = await _httpClient.GetAsync(url);
            if (request.IsSuccessStatusCode)
            {
                string json = request.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<DeliveryDetail>>(json);
            }
            return null!;
        }

        public async Task<ActionResult> GeneratedReportSupplierByProduct(int productId)
        {
            var productSupplierDTO = await GetItemWithSupplierReport(productId);
            if(productSupplierDTO != null)
            {
                var memoryStream = new MemoryStream();
                ProductBySupplierReport report = new(productSupplierDTO, _host);
                report.TitleReport = "reporte de pedidos";
                report.CreateDocument().GeneratePdf(memoryStream);
                memoryStream.Position = 0;
                return File(memoryStream, "application/pdf", $"ReportePedido_{DateTime.Now.ToShortDateString()}.pdf");
            }
            return NotFound("El producto no existe");
        }

        public async Task<ProductSupplierDTO> GetItemWithSupplierReport(int productId)
        {
            HttpResponseMessage httpRequest = await _httpClient.GetAsync($"DeliveryReports/GetSupplierByProductId/{productId}");
            if(httpRequest.IsSuccessStatusCode)
            {
                string json = httpRequest.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ProductSupplierDTO>(json);
            }
            return null!;
        }
    }
}
