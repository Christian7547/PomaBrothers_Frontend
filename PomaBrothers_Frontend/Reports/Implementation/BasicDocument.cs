using Firebase.Auth;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace PomaBrothers_Frontend.Reports.Implementation
{
    public class BasicDocument : Interfaces.IDocument
    {
        public string? TitleReport { get; set; }
        private readonly IWebHostEnvironment _host;

        public BasicDocument(IWebHostEnvironment host)
        {
            _host = host;
        }

        public Document CreateDocument()
        {
            return Document.Create(Compose);
        }

        public void Compose(IDocumentContainer pdf)
        {
            pdf.Page(page =>
            {
                page.Margin(50f);
                page.Header().Element(ComposeHeaderDocument);
                page.Content().Element(ComposeBodyDocument);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" - ").FontSize(10f);
                    text.TotalPages();
                });
            });
        }

        public void ComposeHeaderDocument(IContainer header)
        {
            string path = GetRouteLogo();
            byte[] image = File.ReadAllBytes(path);
            header.Row(row =>
            {
                row.RelativeItem().PaddingLeft(30f).Image(image).FitWidth().FitHeight();
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("HERMANOS POMA").ExtraBold().FontSize(15f);
                    column.Item().PaddingTop(2f).Text("NIT: 12345468794").FontSize(12f).ExtraBold();
                    column.Item().PaddingTop(2f).Text("TELÉFONO: 49498848").FontSize(12f).ExtraBold();

                });
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignCenter().Text(TitleReport!.ToUpper()).FontSize(12f).ExtraBold();
                    column.Item().AlignCenter().PaddingTop(2f).Text(text =>
                    {
                        text.Span("Emitido el ").FontSize(11f);
                        text.Span(DateTime.Now.ToShortDateString().ToString()).FontSize(10f);
                    });
                });
            });
        }

        public string GetRouteLogo()
        {
            var pathLogo = Path.Combine(_host.WebRootPath, "Images/logo.png");
            return pathLogo;
        }

        public virtual void ComposeBodyDocument(IContainer body) => throw new NotImplementedException();

        public virtual void AddDataToDocument(IContainer data) => throw new NotImplementedException();
    }
}