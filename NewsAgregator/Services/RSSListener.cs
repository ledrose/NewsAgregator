using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using NewsAgregator.Data;
using NewsAgregator.Models;

namespace NewsAgregator.Services
{
    public class RSSListener 
    {
        private IServiceProvider _serviceProvider;
        public RSSListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void update()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sources = scope.ServiceProvider.GetService<CustomDbContext>()!.Sources.Where(p => p.Type == SourceType.Rss);
                var list = new List<NewsItem>();
                foreach (var source in sources)
                {
                    var iLastDate = scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems
                        .Where(p => p.SourceName == source.Name)
                        .OrderBy(p => p.CreationDate).LastOrDefault();
                    List<NewsItem> sourceList;
                    var lastDate = DateTime.MinValue;
                    if (iLastDate != null)
                    {
                        lastDate = iLastDate.CreationDate;
                    }
                    try
                    {
                        sourceList = read(source, lastDate);
                        sourceList.ForEach(p => p.SourceName = source.Name);
                        list.AddRange(sourceList);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString() + "Error in :" + source.Name);
                    }

                }

                scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems.AddRange(list);
                scope.ServiceProvider.GetService<CustomDbContext>()!.SaveChanges();
            }
        }

        private List<NewsItem> read(Source source, DateTime lastDate)
        {
            /*
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = resolver;
            */
            var reader = XmlReader.Create(source.Link);
            var feed = SyndicationFeed.Load(reader);
            var list = new List<NewsItem>();
            foreach (var item in feed.Items)
            {
                if (item.PublishDate.UtcDateTime > lastDate)
                {
                    var a = new NewsItem
                    {
                        Category = item.Categories?.FirstOrDefault()?.Name ?? "",
                        CreationDate = item.PublishDate.UtcDateTime,
                        Description = checkHtml(item.Summary?.Text ?? ""),
                        Title = item.Title?.Text ?? "",
                        Url = item.Links?.FirstOrDefault()?.Uri?.OriginalString ?? ""
                    };
                    list.Add(a);
                    //list.Add(_mapper.Map<NewsItem>(item));
                }
            }
            return list;
        }

        private String checkHtml(String str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);
            return doc.DocumentNode.InnerText.Replace("/n", "").Trim();
        }

    }
}
