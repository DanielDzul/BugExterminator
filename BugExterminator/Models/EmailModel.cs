using System.ComponentModel.DataAnnotations;

namespace BugExterminator.Models
{
    public class EmailModel
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}