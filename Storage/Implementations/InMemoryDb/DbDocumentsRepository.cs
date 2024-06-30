using DocumentsService.API.Models;
using DocumentsService.API.Storage.Abstractions;
using DocumentsService.Storage.Implementations.InMemoryDb;
using Microsoft.Extensions.Caching.Memory;

public class DbDocumentsRepository : IDocumentsRepository
{
    private readonly DocumentsDbContext _context;
    private readonly IMemoryCache _cache;

    public DbDocumentsRepository(DocumentsDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Document?> GetDocumentByIdAsync(string id)
    {
        if (_cache.TryGetValue(id, out Document? cachedDocument))
        {
            return cachedDocument;
        }

        var document = await _context.Documents.FindAsync(id);

        if (document != null)
        {
            _cache.Set(id, document, TimeSpan.FromMinutes(10)); 
        }

        return document;
    }

    public async Task SaveDocumentAsync(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        _cache.Set(document.Id, document, TimeSpan.FromMinutes(10));
    }

    public async Task UpdateDocumentAsync(Document document)
    {
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();

        _cache.Set(document.Id, document, TimeSpan.FromMinutes(10));
    }
}
