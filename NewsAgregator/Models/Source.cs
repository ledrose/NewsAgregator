using System.ComponentModel.DataAnnotations;

namespace NewsAgregator.Models
{
    public class Source
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public string RSSUrl { get; set; }
        public List<NewsItem>? NewsItems { get; set; }

    }
}
