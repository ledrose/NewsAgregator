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
            Source source1 = new Source { Name = "Mail.ru", RSSUrl = "https://news.mail.ru/rss/" };
            Source source2 = new Source { Name = "RT", RSSUrl = "https://russian.rt.com/rss" };
            Source sourceу = new Source { Name = "Лента", RSSUrl = "https://lenta.ru/rss" };
            modelBuilder.Entity<Source>().HasData(source1,source2,sourceу);
        }
    }
}
