namespace SecureDocumentAnonymizationSystem.Models
{
    public class Makale
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string TrackingNumber { get; set; } // Add this line  
        public string Status { get; set; }
        public string? Feedback { get; set; }
        public string? ReviewerEmail { get; set; }
        public string? AnonymizedFileName { get; set; }
        public string? ReviewerFeedback { get; set; }


    }
}