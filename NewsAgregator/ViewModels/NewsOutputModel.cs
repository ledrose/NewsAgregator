using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewsAgregator.Models;

namespace NewsAgregator.ViewModels
{
    public class NewsOutputModel
    {
        public List<NewsOutputItemModel> Items { get; set; }
        public bool Final { get; set; }
    }

    public class NewsOutputItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Unknown";
        public string Url { get; set; } = "";
        public string Category { get; set; } = "Unknown";
        public string Description { get; set; } = "";
        public string CreationDate { get; set; } = "";
        public String SourceName { get; set; } = "";
    }
}
