using PomaBrothers_Frontend.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PomaBrothers_Frontend.Reports.Implementation.DeliveryReports
{
    public class OrdersByDateRangeReport : BasicDocument
    {
        private DateTime StartDate { get; set; }
        private DateTime FinishDate { get; set; }
        private decimal Total { get; set; }
        private List<DeliveryDetail> Items { get; set; }

        public OrdersByDateRangeReport(DateTime startDate, DateTime finishDate, decimal total, List<DeliveryDetail> items, IWebHostEnvironment host)
            : base(host)
        {
            StartDate = startDate;
            FinishDate = finishDate;
            Total = total;
            Items = items;
        }

        public override void ComposeBodyDocument(IContainer body)
        {
            body.Column(column =>
            {
                column.Item().PaddingTop(20f).AlignCenter()
                    .Text($"Productos adquiridos desde {StartDate.ToShortDateString()} a {FinishDate.ToShortDateString()}".ToUpper())
                    .ExtraBold()
                    .FontSize(10f);
                column.Item().PaddingTop(20f).AlignLeft()
                    .Text("PRODUCTOS")
                    .ExtraBold()
                    .FontSize(12f);
                column.Item().PaddingTop(7f).Element(AddDataToDocument);
                column.Item().AlignRight().PaddingTop(4f)
                    .Text($"Inversión: {Total}$")
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

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Producto");
                    header.Cell().Element(CellStyle).AlignRight().Text("N° serie");
                    header.Cell().Element(CellStyle).AlignRight().Text("Modelo");
                    header.Cell().Element(CellStyle).AlignRight().Text("Marca");
                    header.Cell().Element(CellStyle).AlignRight().Text("Precio/compra");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach(var item in Items)
                {
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).AlignLeft().Text(item.Item.Name);
                    table.Cell().Element(CellStyle).Text(item.Item.Serie);
                    table.Cell().Element(CellStyle).Text(item.Item.ItemModel.ModelName);
                    table.Cell().Element(CellStyle).Text(item.Item.ItemModel.Marker);
                    table.Cell().Element(CellStyle).Text(item.PurchasePrice.ToString());
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).AlignRight();
                    }
                }
            });
        }
    }
}
