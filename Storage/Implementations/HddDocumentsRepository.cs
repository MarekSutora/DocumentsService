using DocumentsService.API.DTOs.Request;
using DocumentsService.API.Models;
using DocumentsService.API.Storage.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DocumentsService.API.Storage.Implementations
{
    public class HddDocumentsRepository : IDocumentsRepository
    {
        public HddDocumentsRepository() { }

        public async Task<Document?> GetDocumentByIdAsync(string id)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(currentDirectory, "Storage", "Temp");
            string[] filePaths = Directory.GetFiles(tempDirectory, "*.json");


            foreach (var filePath in filePaths)
            {
                try
                {
                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    var document = JsonSerializer.Deserialize<Document>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (document is not null && document.Id == id)
                    {
                        return document;
                    }
                }
                catch (JsonException ex)
                {
                    // Log the exception (implement logging as needed)
                    Console.WriteLine($"Error deserializing file {filePath}: {ex.Message}");
                }
                catch (IOException ex)
                {
                    // Log the exception (implement logging as needed)
                    Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                }
            }


            return null;
        }

        public async Task SaveDocumentAsync(Document document)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string directoryPath = Path.Combine(currentDirectory, "Storage", "Temp");
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, $"{document.Id}.json");
            string jsonContent = JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, jsonContent);
        }

        public Task UpdateDocumentAsync(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
