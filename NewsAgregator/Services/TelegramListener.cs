using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsAgregator.Data;
using NewsAgregator.Models;
using System.Collections.Generic;
using System.Threading.Channels;
using TL;
using WTelegram;
using static System.Net.WebRequestMethods;

namespace NewsAgregator.Services
{

    public class TelegramListener 
    {
        private IServiceProvider _serviceProvider;
        private Client _client;
        private List<Category> _categories = new List<Category>();
        private List<NewsItem> _items = new List<NewsItem>();
        public TelegramListener(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _client = new Client(25875797, "e1f209d3dc7403cbeaa33b003f7d9c44");
        }
        public async Task Update()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sources = scope.ServiceProvider.GetService<CustomDbContext>()!.Sources.Include(p => p.Categories).Where(p=>p.Type==SourceType.Telegram);
                var list = new List<NewsItem>();
                foreach (var source in sources)
                {
                    var iLastDate = scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems
                        .Where(p => p.SourceName == source.Name)
                        .OrderBy(p => p.CreationDate).LastOrDefault();
                    var lastDate = iLastDate?.CreationDate ?? DateTime.MinValue;
                    try
                    {
                        await read(source, lastDate);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString() + "Error in @" + source.Name);
                    }

                }
                scope.ServiceProvider.GetService<CustomDbContext>()!.Categories.AddRange(_categories);
                scope.ServiceProvider.GetService<CustomDbContext>()!.NewsItems.AddRange(_items);
                scope.ServiceProvider.GetService<CustomDbContext>()!.SaveChanges();
            }
        }
       
        private async Task read(Source source, DateTime lastDate)
        {
            var channel = (TL.Channel)(await _client.Contacts_ResolveUsername(source.Link)).Chat;
            var channelInput = new InputChannel(channel.ID, channel.access_hash);
            await _client.Channels_JoinChannel(channelInput);
            await _client.Channels_ReadHistory(channelInput);
            var lastMsgId = ((ChannelFull)(await _client.Channels_GetFullChannel(channelInput)).full_chat).read_inbox_max_id;
            var mesArr = new List<InputMessageID>();
            Console.WriteLine(Enumerable.Range(lastMsgId - 10, lastMsgId + 1));
            Enumerable.Range(lastMsgId - 19, 20).ToList().ForEach((i) => mesArr.Add((InputMessageID)(new InputMessageID().id = i)));
            var mesList = (Messages_ChannelMessages)(await _client.Channels_GetMessages(channelInput, mesArr.ToArray()));

            foreach (var item in mesList.messages)
            {
                try
                {
                    var curItem = (Message)item;
                    if (curItem.Date > lastDate && curItem.message != "")
                    {
                        var catName = "";
                        if (source.Categories.Where(e => e.Name == catName).IsNullOrEmpty() &&
                            _categories.Where(e => e.Name == catName).IsNullOrEmpty())
                        {
                            _categories.Add(new Category
                            {
                                Name = catName,
                                SourceName = source.Name,
                            });
                        }
                        var a = new NewsItem
                        {
                            SourceName = source.Name,
                            CategoryName = catName,
                            CreationDate = curItem.Date.ToUniversalTime(),
                            Description = curItem.message ?? "",
                            Url = "https://t.me/" + source.Link + "/" + curItem.ID
                        };
                        _items.Add(a);
                    }
                }
                catch (Exception ex){
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public async Task DoLogin(string loginInfo)
        {
            while (_client.User == null)
                switch (await _client.Login(loginInfo)) 
                {
                    case "verification_code": Console.Write("Code: "); loginInfo = Console.ReadLine(); break;
                    //case "  name": loginInfo = "John Doe"; break;   
                    //case "password": loginInfo = "secret!"; break; 
                    default: loginInfo = null; break;
                }
            Console.WriteLine($"We are logged-in as {_client.User} (id {_client.User.id})");
        }

    }
}
