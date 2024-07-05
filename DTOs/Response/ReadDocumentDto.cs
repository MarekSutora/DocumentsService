

namespace DocumentsService.API.DTOs.Response
{
    public class ReadDocumentDto
    {
        public required string Id { get; set; }
        public required List<string> Tags { get; set; }
        public required Dictionary<string, object> Data { get; set; }
    }
}
