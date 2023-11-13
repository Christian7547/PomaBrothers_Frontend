﻿using Microsoft.AspNetCore.Mvc;
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
            if (itemsByDateRange != null)
            {
                var total = itemsByDateRange.Sum(purchasePrice => purchasePrice.PurchasePrice);
                try
                {
                    var memoryStream = new MemoryStream();
                    OrdersByDateRangeReport report = new(startDate, finishDate, total, itemsByDateRange, _host);
                    report.TitleReport = "reporte\npedidos";
                    report.CreateDocument().GeneratePdf(memoryStream);
                    memoryStream.Position = 0;
                    return File(memoryStream.ToArray(), "application/pdf");
                }
                catch
                {
                    throw;
                }
            }
            return NotFound("There are no orders placed in that date range");
        }


        public async Task<List<DeliveryDetail>> GetOrdersByDateRangeReport(DateTime startDate, DateTime finishDate)
        {
            string formattedStartDate = Uri.EscapeDataString(startDate.ToString("MM/dd/yyyy"));
            string formattedFinishDate = Uri.EscapeDataString(finishDate.ToString("MM/dd/yyyy"));
            string url = $"DeliveryReports/GetOrdersByDateRangeReport?startDate={formattedStartDate}&endDate={formattedFinishDate}";
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
                report.TitleReport = "reporte\npedidos";
                report.CreateDocument().GeneratePdf(memoryStream);
                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "application/pdf");
            }
            return NotFound();
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