namespace placement.Models
{
    public class QueryDto
    {
        public int? TaskId { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        
        public IFormFile? Attachment { get; set; }
    }
}
