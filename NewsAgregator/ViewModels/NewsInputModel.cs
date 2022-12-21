﻿namespace NewsAgregator.ViewModels
{
    public class NewsInputModel
    {
        public int Amount { get; set; } = 20;
        public int StartIndex { get; set; } = 0;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public string SearchQuery { get; set; } = "";
        public List<String> Sources { get; set; } = new List<String>();
    }
}
