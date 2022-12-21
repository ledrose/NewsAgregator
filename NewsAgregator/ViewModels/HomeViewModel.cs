using Microsoft.AspNetCore.Mvc;

namespace NewsAgregator.ViewModels
{
    public class HomeViewModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SearchQuery { get; set; }
    }
}
