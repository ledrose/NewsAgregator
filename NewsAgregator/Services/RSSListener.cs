using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using NewsAgregator.Data;
using NewsAgregator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace NewsAgregator.Services
{
    public class RSSListener 
    {
        private IServiceProvider _serviceProvider;
        private List<Category> _categories = new List<Category>();
        private List<NewsItem> _items = new List<NewsItem>();
        public RSSListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Update()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sources = scope.ServiceProvider.GetService<CustomDbContext>()!.Sources.Include(p=> p.Categories).Where(p => p.Type == SourceType.Rss).ToList();
                foreach (var source in sources)
                {
                    var iLastDate = scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems
                        .Where(p => p.SourceName == source.Name)
                        .OrderBy(p => p.CreationDate).LastOrDefault();
                    var lastDate = iLastDate?.CreationDate ?? DateTime.MinValue;
                    try
                    {
                        read(source, lastDate);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString() + "Error in :" + source.Name);
                    }

                }
                scope.ServiceProvider.GetService<CustomDbContext>()!.Categories.AddRange(_categories);
                scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems.AddRange(_items);
                scope.ServiceProvider.GetService<CustomDbContext>()!.SaveChanges();
            }
        }

        private void read(Source source, DateTime lastDate)
        {
            var reader = XmlReader.Create(source.Link);
            var feed = SyndicationFeed.Load(reader);
            foreach (var item in feed.Items)
            {
                if (item.PublishDate.UtcDateTime > lastDate)
                {
                    var catName = item.Categories?.FirstOrDefault()?.Name ?? "";
                    if (source.Categories!.Where(e => e.Name == catName).IsNullOrEmpty() &&
                        _categories!.Where(e => e.Name == catName).IsNullOrEmpty()) 
                    {
                        _categories.Add(new Category { 
                            Name= catName,
                            SourceName= source.Name,
                        });
                    } 
                    var a = new NewsItem
                    {
                        CreationDate = item.PublishDate.UtcDateTime,
                        Description = checkHtml(item.Summary?.Text ?? ""),
                        Title = item.Title?.Text ?? "",
                        Url = item.Links?.FirstOrDefault()?.Uri?.OriginalString ?? "",
                        SourceName = source.Name,
                        CategoryName = catName
                    };
                    _items.Add(a);
                }
            }
        }

        private String checkHtml(String str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);
            return doc.DocumentNode.InnerText.Replace("/n", "").Trim();
        }

    }
}
