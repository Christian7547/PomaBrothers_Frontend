using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PomaBrothers_Frontend.Reports.Interfaces
{
    public interface IDocument
    {
        Document CreateDocument();
        void Compose(IDocumentContainer pdf);
        void ComposeHeaderDocument(IContainer header);
        void ComposeBodyDocument(IContainer body); 
        void AddDataToDocument(IContainer data);
        string GetRouteLogo();
    }
}