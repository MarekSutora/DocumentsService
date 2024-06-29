namespace DocumentsService.DTOs.Request
{
    public class UpdateDocumentDto
    {
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
