using PomaBrothers_Frontend.Models;
using PomaBrothers_Frontend.Models.DTOModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PomaBrothers_Frontend.Reports.Implementation.SaleReports
{
    public class SalesByDateRangeReport : BasicDocument
    {
        private DateTime StartDate { get; set; }
        private DateTime FinishDate { get; set; }
        private decimal Total { get; set; }
        private List<SaleDTO> SaleDTOs { get; set; }

        public SalesByDateRangeReport(DateTime startDate, DateTime finishDate, decimal total, List<SaleDTO> saleDTOs, IWebHostEnvironment host) 
            : base(host)
        {
            StartDate = startDate;
            FinishDate = finishDate;
            Total = total;
            SaleDTOs = saleDTOs;
        }

        public override void ComposeBodyDocument(IContainer body)
        {
            body.Column(column =>
            {
                column.Item().PaddingTop(20f).AlignCenter()
                    .Text($"Productos vendidos desde {StartDate.ToShortDateString()} a {FinishDate.ToShortDateString()}".ToUpper())
                    .ExtraBold()
                    .FontSize(10f);
                column.Item().PaddingTop(20f).AlignLeft()
                    .Text("HISTORIAL DE VENTAS")
                    .ExtraBold()
                    .FontSize(12f);
                column.Item().PaddingTop(7f).Element(AddDataToDocument);
                column.Item().AlignRight().PaddingTop(4f)
                    .Text($"Ganancia: {Total} Bs.")
                    .FontSize(12f)
                    .Bold();
            });
        }

        public override void AddDataToDocument(IContainer data)
        {
            data.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                var groups = SaleDTOs.GroupBy(sale => sale.RegisterDate.ToShortDateString());
                foreach(var group in groups)
                {
                    table.Cell().ColumnSpan(5).PaddingTop(25).BorderBottom(1).Text($"Ventas de {group.Key}".ToUpper()).ExtraBold();
                    foreach (var saleDTO in group)
                    {
                        if(saleDTO.Products.Count > 0)
                        {
                            table.Cell().ColumnSpan(5).PaddingVertical(5).Text("Productos vendidos").ExtraBold();
                            table.Cell().AlignLeft().Element(CellTotal).Text("Nombre").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text("N° serie").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text("Modelo").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text("Marca").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text("Precio/venta").Bold();
                            foreach (var product in saleDTO.Products)
                            {
                                table.Cell().Element(CellStyleTable).Text(product.NameProduct);
                                table.Cell().Element(CellStyleTable).AlignRight().Text(product.Serie);
                                table.Cell().Element(CellStyleTable).AlignRight().Text(product.ModelNameProduct);
                                table.Cell().Element(CellStyleTable).AlignRight().Text(product.MarkerProduct);
                                table.Cell().Element(CellStyleTable).AlignRight().Text($"{product.PriceProduct} Bs.");

                                static IContainer CellStyleTable(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                            }
                            table.Cell().ColumnSpan(3);
                            table.Cell().AlignRight().Element(CellTotal).Text("Recaudación:").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text($"{saleDTO.Total} Bs.");
                            static IContainer CellTotal(IContainer container)
                            {
                                return container.PaddingVertical(5);
                            }
                        }
                        else
                        {
                            table.Cell().ColumnSpan(5).PaddingVertical(5).Text("No hubo ventas esta fecha").ExtraBold();
                            table.Cell().ColumnSpan(3);
                            table.Cell().AlignRight().Element(CellTotal).Text("Recaudación:").Bold();
                            table.Cell().AlignRight().Element(CellTotal).Text("0 Bs.");
                            static IContainer CellTotal(IContainer container)
                            {
                                return container.PaddingVertical(5);
                            }
                        }
                    }
                }
            });
        }
    }
}
