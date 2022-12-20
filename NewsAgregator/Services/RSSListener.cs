using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ServiceModel.Syndication;
using System.Xml;
using VueProjectBack.Data;
using VueProjectBack.Models;

namespace VueProjectBack.Services 
{
    public class RSSListener : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        public RSSListener(IServiceProvider serviceProvider,IMapper mapper) {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                update();
                await Task.Delay(new TimeSpan(0, 30, 0));
            }
        }
        public void update()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sources = scope.ServiceProvider.GetService<CustomDbContext>()!.Sources;
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
                        sourceList = readUrl(source, lastDate);
                        sourceList.ForEach(p => p.SourceName = source.Name);
                        list.AddRange(sourceList);
                    }           
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString()+"Error in :"+ source.Name);
                    }
                    
                }

                scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems.AddRange(list);
                scope.ServiceProvider.GetService<CustomDbContext>()!.SaveChanges();
            }
        }

        private List<NewsItem> readUrl(Source source, DateTime lastDate)
        {
            var reader = XmlReader.Create(source.RSSUrl);
            var feed =SyndicationFeed.Load(reader);
            var list = new List<NewsItem>();
            foreach (var item in feed.Items)
            {
                if (item.PublishDate.DateTime>lastDate)
                {
                    var a = new NewsItem
                    {
                        Category = item.Categories?.First().Name ?? "",
                        CreationDate = item.PublishDate.DateTime,
                        Description = item.Summary?.Text ?? "",
                        Title = item.Title?.Text ?? "",
                        Url = item.Links?.First().Uri?.OriginalString ?? "",
                    };
                    list.Add(a);
                    //list.Add(_mapper.Map<NewsItem>(item));
                }
            }
            return list;
        }

    }
}
