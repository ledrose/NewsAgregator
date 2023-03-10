using NewsAgregator.Models;

namespace NewsAgregator.ViewModels
{
    public class SourceInputModel
    {
        public string Name { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
    public class NewsInputModel
    {
        public int Amount { get; set; } = 20;
        public int StartIndex { get; set; } = 0;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public int TimeOffsetInHours { get; set; } = 0;
        public string SearchQuery { get; set; } = "";
        public List<SourceInputModel> Sources { get; set; } = new List<SourceInputModel>();
    }
}
