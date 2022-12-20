using Microsoft.EntityFrameworkCore;
using VueProjectBack.Models;

namespace VueProjectBack.Data
{
     public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<NewsItem> NewsItems { get; set; } = null!;
        public DbSet<Source> Sources { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NewsItem item1 = new NewsItem {Id=1, SourceName="Mail.ru", CreationDate = DateTime.MinValue};
            Source source1 = new Source { Name = "Mail.ru", RSSUrl = "https://news.mail.ru/rss/" };
            Source source2 = new Source { Name = "RT", RSSUrl = "https://russian.rt.com/rss" };
            Source source3 = new Source { Name = "Медуза", RSSUrl = "https://meduza.io/rss/all" };
            Source source4 = new Source { Name = "РИА новости", RSSUrl = "https://ria.ru/export/rss2/archive/index.xml" };
            Source source5 = new Source { Name = "Лента", RSSUrl = "https://lenta.ru/rss" };
            modelBuilder.Entity<NewsItem>().HasData(item1);
            modelBuilder.Entity<Source>().HasData(source1,source2,source3,source4,source5);
        }
    }
}
