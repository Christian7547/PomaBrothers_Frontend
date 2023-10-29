using PomaBrothers_Frontend.Models.DTOModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PomaBrothers_Frontend.Reports.Implementation.DeliveryReports
{
    public class ProductBySupplierReport : BasicDocument
    {
        private ProductSupplierDTO _productSupplierDTO { get; set; }

        public ProductBySupplierReport(ProductSupplierDTO productSupplierDTO, IWebHostEnvironment host)
            :base(host)
        {
            _productSupplierDTO = productSupplierDTO;
        }

        public override void ComposeBodyDocument(IContainer body)
        {
            body.Column(column =>
            {
                column.Item().PaddingTop(20).AlignCenter().Text($"Proveedor de un producto".ToUpper())
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
                });

                table.Cell().ColumnSpan(4).PaddingTop(25).BorderBottom(1).Text("Proveedor".ToUpper()).ExtraBold();

                table.Cell().Element(CellStyleData).Text("Nombre:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.BussinesName);

                table.Cell().Element(CellStyleData).Text("Número de teléfono:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.SupplierPhone);

                table.Cell().Element(CellStyleData).Text("NIT:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.SupplierNit);

                table.Cell().Element(CellStyleData).Text("Manager:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.Manager);

                table.Cell().Element(CellStyleData).Text("Dirección:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.SupplierAddress);



                table.Cell().ColumnSpan(4).PaddingTop(25).BorderBottom(1).Text("Producto".ToUpper()).ExtraBold();

                table.Cell().Element(CellStyleData).Text("Producto:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.ItemName);

                table.Cell().Element(CellStyleData).Text("Modelo:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.ItemModelName);

                table.Cell().Element(CellStyleData).Text("Marca:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.ItemMarker);

                table.Cell().Element(CellStyleData).Text("Serie:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.Serie);

                table.Cell().Element(CellStyleData).Text("Precio de compra:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.PurchasePrice.ToString());

                table.Cell().Element(CellStyleData).Text("Adquirido el ").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.RegisterDateItem.ToShortDateString());

                table.Cell().Element(CellStyleData).Text("Garantía:").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text($"{_productSupplierDTO.DurationWarranty} {_productSupplierDTO.TypeWarranty}");

                table.Cell().Element(CellStyleData).Text("Descripción del producto: ").Bold();
                table.Cell().ColumnSpan(3).Element(CellStyleData).Text(_productSupplierDTO.ItemDescription);

                static IContainer CellStyleData(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(6);
                }
            });
        }
    }
}