using System.ComponentModel.DataAnnotations;

namespace NewsAgregator.Models
{
    public enum SourceType
    {
        [Display(Name = "Rss")]
        Rss, 
        [Display(Name = "Телеграмм")]
        Telegram
    }
    public class Source
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public SourceType Type { get; set; }
        [Required]
        public string Link { get; set; }
        public virtual List<NewsItem>? NewsItems { get; set; }
        public virtual List<Category>? Categories { get; set; }
    }
}
