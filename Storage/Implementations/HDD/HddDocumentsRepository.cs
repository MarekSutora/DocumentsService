using DocumentsService.API.DTOs.Request;
using DocumentsService.API.Models;
using DocumentsService.API.Storage.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace DocumentsService.Storage.Implementations.HDD
{
    public class HddDocumentsRepository : IDocumentsRepository
    {
        private readonly IMemoryCache _cache;
        private readonly string documentsDirectory;

        public HddDocumentsRepository(IMemoryCache cache)
        {
            _cache = cache;
            string currentDirectory = Directory.GetCurrentDirectory();
            documentsDirectory = Path.Combine(currentDirectory, "Storage", "Implementations", "HDD", "Documents");
            Directory.CreateDirectory(documentsDirectory);
        }

        public async Task<Document?> GetDocumentByIdAsync(string id)
        {
            if (_cache.TryGetValue(id, out Document? cachedDocument))
            {
                return cachedDocument;
            }

            string filePath = Path.Combine(documentsDirectory, $"{id}.json");

            if (File.Exists(filePath))
            {
                try
                {
                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    var document = JsonSerializer.Deserialize<Document>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (document != null)
                    {
                        _cache.Set(id, document, TimeSpan.FromMinutes(10));
                    }

                    return document;
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"Error deserializing document from file {filePath}: {ex.Message}");
                }
                catch (IOException ex)
                {
                    throw new IOException($"Error reading document from file {filePath}: {ex.Message}");
                }
            }

            return null;
        }

        public async Task SaveDocumentAsync(Document document)
        {
            string filePath = Path.Combine(documentsDirectory, $"{document.Id}.json");
            string jsonContent = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, jsonContent);

            _cache.Set(document.Id, document, TimeSpan.FromMinutes(10)); 
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            string filePath = Path.Combine(documentsDirectory, $"{document.Id}.json");

            if (File.Exists(filePath))
            {
                string jsonContent = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, jsonContent);

                _cache.Set(document.Id, document, TimeSpan.FromMinutes(10)); 
            }
            else
            {
                throw new FileNotFoundException($"Document with ID {document.Id} does not exist.");
            }
        }
    }
}
