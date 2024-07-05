using System.ComponentModel.DataAnnotations;

namespace DocumentsService.API.DTOs.Request
{
    public class UpdateDocumentDto
    {
        [Required(ErrorMessage = "Id is required!")]
        public required string Id { get; set; }
        [Required(ErrorMessage = "Tags are required!")]
        public required List<string> Tags { get; set; }
        [Required(ErrorMessage = "Data is required!")]
        public required Dictionary<string, object> Data { get; set; }
    }
}
