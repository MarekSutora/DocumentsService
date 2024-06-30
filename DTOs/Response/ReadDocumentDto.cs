namespace DocumentsService.DTOs.Response
{
    public class ReadDocumentDto
    {
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
