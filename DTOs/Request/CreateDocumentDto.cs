using DocumentsService.API.Models;
using System.ComponentModel.DataAnnotations;

namespace DocumentsService.API.DTOs.Request
{
    public class CreateDocumentDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public List<string> Tags { get; set; }
        [Required]
        public required Dictionary<string, object> Data { get; set; }
    }
}
