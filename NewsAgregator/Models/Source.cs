﻿using System.ComponentModel.DataAnnotations;

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
        public string Link { get; set; }
        public List<NewsItem>? NewsItems { get; set; }
    }
}
