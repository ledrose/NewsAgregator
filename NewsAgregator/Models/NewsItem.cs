using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAgregator.Models
{
    public class NewsItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Column(Order = 1)]
        public string? SourceName { get; set; }
        [Column(Order = 2)]
        public string? CategoryName { get; set; } = "";
        [ForeignKey("SourceName")]
        public virtual Source Source { get; set; } = null!;
        [ForeignKey("SourceName, CategoryName")]
        public virtual Category? Category { get; set; }
    }
}
