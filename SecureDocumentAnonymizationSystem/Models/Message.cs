using System;

namespace SecureDocumentAnonymizationSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }  // Makaleye ait takip numarası
        public string Text { get; set; }
        public string Sender { get; set; } // "author" veya "editor"
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
