using System.ComponentModel.DataAnnotations;

namespace DocumentsService.DTOs.Request
{
    public class UpdateDocumentDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public List<string> Tags { get; set; }
        [Required]
        public Dictionary<string, object> Data { get; set; }
    }
}
