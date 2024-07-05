
namespace DocumentsService.API.Models
{
    public class Document
    {

        public required string Id { get; set; }
        public required List<string> Tags { get; set; }
        public required Dictionary<string, object> Data { get; set; }
    }
}
