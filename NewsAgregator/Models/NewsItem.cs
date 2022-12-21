using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAgregator.Models
{
    public class NewsItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = "Unknown";
        public string Url { get; set; } = "";
        public string Category { get; set; } = "Unknown";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public String SourceName { get; set; }
        [ForeignKey("SourceName")]
        public Source Source { get; set; } = null!;
    }
}
