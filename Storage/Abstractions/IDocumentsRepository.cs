using DocumentsService.API.Models;

namespace DocumentsService.API.Storage.Abstractions
{
    public interface IDocumentsRepository
    {
        Task<Document?> GetDocumentByIdAsync(string id);
        Task SaveDocumentAsync(Document document);
        Task UpdateDocumentAsync(Document document);
    }
}
