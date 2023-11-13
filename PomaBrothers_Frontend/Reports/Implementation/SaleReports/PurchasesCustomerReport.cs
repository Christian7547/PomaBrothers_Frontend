using PomaBrothers_Frontend.Models.DTOModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PomaBrothers_Frontend.Reports.Implementation.SaleReports
{
    public class PurchasesCustomerReport : BasicDocument
    {
        private PurchasesCustomerDTO _purchases { get; set; }

        public PurchasesCustomerReport(IWebHostEnvironment host, PurchasesCustomerDTO purchases)
            :base(host)
        {
            _purchases = purchases;
        }

        public override void ComposeBodyDocument(IContainer body)
        {
            body.Column(column =>
            {
                column.Item().PaddingTop(20).AlignCenter().Text($"compras hechas por {_purchases.CompleteNameCustomer}".ToUpper())
                .ExtraBold().FontSize(10);

                column.Item().PaddingTop(7).Element(AddDataToDocument);
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

                table.Cell().ColumnSpan(5).PaddingTop(25).BorderBottom(1).Text("detalles del cliente".ToUpper()).ExtraBold();

                table.Cell().Element(CellStyleData).Text("Nombre completo:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_purchases.CompleteNameCustomer);

                table.Cell().Element(CellStyleData).Text("C.I:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_purchases.CiCustomer);

                table.Cell().Element(CellStyleData).Text("Correo Electrónico:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_purchases.EmailCustomer);

                static IContainer CellStyleData(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(6);

                table.Cell().ColumnSpan(5).PaddingTop(25).PaddingBottom(10).Text("historial de compras".ToUpper()).ExtraBold();
                table.Cell().ColumnSpan(5).PaddingTop(15).Text($"Fecha de compra: {_purchases.SaleDTO.RegisterDate}".ToUpper()).FontSize(10).ExtraBlack();
                table.Cell().ColumnSpan(5).PaddingVertical(5).Text("Productos adquiridos").ExtraBold();
                foreach (var product in _purchases.SaleDTO.Products)
                {
                    table.Cell().Element(CellStyleTable).Text(product.NameProduct);
                    table.Cell().Element(CellStyleTable).AlignRight().Text(product.Serie);
                    table.Cell().Element(CellStyleTable).AlignRight().Text(product.ModelNameProduct);
                    table.Cell().Element(CellStyleTable).AlignRight().Text(product.MarkerProduct);
                    table.Cell().Element(CellStyleTable).AlignRight().Text($"{product.PriceProduct} Bs.");

                    static IContainer CellStyleTable(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
                table.Cell().ColumnSpan(3);
                table.Cell().AlignRight().Element(CellTotal).Text("Total:").Bold();
                table.Cell().AlignRight().Element(CellTotal).Text($"{_purchases.SaleDTO.Total} Bs.");

                static IContainer CellTotal(IContainer container)
                {
                    return container.PaddingVertical(5);
                }
            });
        }
    }
}