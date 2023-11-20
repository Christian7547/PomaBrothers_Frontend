
using PomaBrothers_Frontend.Models.DTOModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.PortableExecutable;

namespace PomaBrothers_Frontend.Reports.Implementation.DeliveryReports
{
    public class ProductsBySupplierBetweenDatesReport : BasicDocument
    {
        private SupplierItemsDTO _supplierItems { get; set; }
        private DateTime _startDate { get; set; }    
        private DateTime _endDate { get; set; }

        public ProductsBySupplierBetweenDatesReport(IWebHostEnvironment host, SupplierItemsDTO supplierItems, DateTime startDate, DateTime endDate) 
            : base(host)
        {
            _supplierItems = supplierItems;
            _startDate = startDate;
            _endDate = endDate;
        }

        public override void ComposeBodyDocument(IContainer body)
        {
            body.Column(column =>
            {
                column.Item()
                .PaddingTop(20)
                .AlignCenter()
                .Text($"Productos adquiridos de un proveedor desde {_startDate.ToShortDateString()} a {_endDate.ToShortDateString()}".ToUpper())
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

                table.Cell().ColumnSpan(5).PaddingTop(25).BorderBottom(1).Text("Proveedor".ToUpper()).ExtraBold();

                table.Cell().Element(CellStyleData).Text("Nombre:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_supplierItems.BussinesNameSupplier);

                table.Cell().Element(CellStyleData).Text("Número de teléfono:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_supplierItems.PhoneSupplier);

                table.Cell().Element(CellStyleData).Text("NIT:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_supplierItems.CiSupplier);

                table.Cell().Element(CellStyleData).Text("Manager:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_supplierItems.ManagerSupplier);

                table.Cell().Element(CellStyleData).Text("Dirección:").Bold();
                table.Cell().ColumnSpan(4).Element(CellStyleData).Text(_supplierItems.AddressSupplier);

                static IContainer CellStyleData(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(6);
                }

                table.Cell().ColumnSpan(5).PaddingTop(25).BorderBottom(1).Text("Productos".ToUpper()).ExtraBold();

                var groupByDates = _supplierItems.Products.GroupBy(g => g.RegisterDateProduct.ToShortDateString());
                foreach(var group in groupByDates)
                {
                    table.Cell().ColumnSpan(5).PaddingTop(12).Text($"Productos adquiridos el {group.Key}").SemiBold();

                    table.Cell().Element(CellStyle).Text("Producto");
                    table.Cell().Element(CellStyle).AlignRight().Text("N° serie");
                    table.Cell().Element(CellStyle).AlignRight().Text("Modelo");
                    table.Cell().Element(CellStyle).AlignRight().Text("Marca");
                    table.Cell().Element(CellStyle).AlignRight().Text("Precio/compra");
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                    foreach (var item in group)
                    {
                        table.Cell().Element(CellStyleTable).Text(item.NameProduct);
                        table.Cell().Element(CellStyleTable).AlignRight().Text(item.Serie);
                        table.Cell().Element(CellStyleTable).AlignRight().Text(item.ModelNameProduct);
                        table.Cell().Element(CellStyleTable).AlignRight().Text(item.MarkerProduct);
                        table.Cell().Element(CellStyleTable).AlignRight().Text($"{item.PriceProduct} Bs.");
                        static IContainer CellStyleTable(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }
    }
}
