using DocumentsService;
using DocumentsService.API.Models;
using DocumentsService.API.Storage.Abstractions;
using DocumentsService.Storage.Implementations.InMemoryDb;
using System.Text.Json;

public class InMemoryDocumentsRepository : IDocumentsRepository
{
    private readonly DocumentsDbContext _context;

    public InMemoryDocumentsRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task AddDocumentAsync(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
    }

    public async Task<Document?> GetDocumentByIdAsync(string id)
    {
        var document = await _context.Documents.FindAsync(id);

        return document;
    }

    public async Task SaveDocumentAsync(Document document)
    {
        await AddDocumentAsync(document);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDocumentAsync(Document document)
    {
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();
    }
}
