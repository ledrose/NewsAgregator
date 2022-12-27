using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAgregator.Models
{
    public class Category
    {
        [Column(Order = 1)]
        public string SourceName { get; set; }
        [Column(Order = 2)]
        public string Name { get; set; }
        [ForeignKey("SourceName")]
        public virtual Source Sourse { get; set; }
        public virtual List<NewsItem>? NewsItems { get; set; }
    }
}