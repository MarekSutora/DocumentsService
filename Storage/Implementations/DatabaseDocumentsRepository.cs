using DocumentsService.API.Models;
using DocumentsService.API.Storage.Abstractions;

namespace DocumentsService.API.Storage.Implementations
{
    public class DatabaseDocumentsRepository : IDocumentsRepository
    {
        public DatabaseDocumentsRepository()
        {
        }

        public Task<Document?> GetDocumentByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task SaveDocumentAsync(Document document)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDocumentAsync(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
