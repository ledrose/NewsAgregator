using System.ComponentModel.DataAnnotations;

namespace VueProjectBack.Models
{
    public class Source
    {
        [Key]
        public string Name { get; set; }
        public string RSSUrl { get; set; }
        public List<NewsItem> NewsItems { get; set; }

    }
}
