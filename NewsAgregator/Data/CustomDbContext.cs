using Microsoft.EntityFrameworkCore;
using NewsAgregator.Models;

namespace NewsAgregator.Data
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
            Source source1 = new Source { Name = "Mail.ru",Type=SourceType.Rss, Link = "https://news.mail.ru/rss/" };
            Source source2 = new Source { Name = "RT", Type = SourceType.Rss, Link = "https://russian.rt.com/rss" };
            Source source3 = new Source { Name = "Лента", Type = SourceType.Rss, Link = "https://lenta.ru/rss" };
            Source source4 = new Source { Name = "Астрей", Type = SourceType.Telegram, Link = "astrey" };
            modelBuilder.Entity<Source>().HasData(source1,source2,source3,source4);
        }
    }
}
