using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAgregator.Data;
using Newtonsoft.Json.Linq;
using NewsAgregator.ViewModels;
using AutoMapper;

namespace NewsAgregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private CustomDbContext _db;
        private IMapper _mapper;
        private readonly DateTime minDateTime = new DateTime(2010, 1, 1);
        public NewsController(CustomDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<NewsOutputModel>> GetNewsItems([FromBody]NewsInputModel data)
        {
            if (_db.NewsItems == null)
            {
                return BadRequest("Empty table");
            }
            try
            {
                int amount = data.Amount;
                if (amount < 1 || amount > 30) amount = 30;
                int startIndex = data.StartIndex;
                var sources = data.Sources;
                var startTime =  data.StartTime;
                if (data.StartTime>=minDateTime)
                {
                    startTime = data.StartTime.AddHours(-data.TimeOffsetInHours);
                }
                var endTime = DateTime.Now;
                if (data.EndTime>minDateTime)
                {
                    endTime = data.EndTime.AddHours(-data.TimeOffsetInHours);
                }                
                var items = _db.NewsItems
                    .Where(p => 
                        data.Sources.Contains(p.SourceName) &&
                        p.CreationDate>startTime &&
                        p.CreationDate<endTime &&
                        p.Title.Contains(data.SearchQuery))
                    .OrderByDescending(p => p.CreationDate).Skip(startIndex).Take(amount)
                    .ToList();
                items.ForEach(p => p.CreationDate = p.CreationDate.AddHours(data.TimeOffsetInHours));
                var itemsMapped = new List<NewsOutputItemModel>();
                items.ForEach(item => itemsMapped.Add(_mapper.Map<NewsOutputItemModel>(item)));
                var output = new NewsOutputModel {
                    Final = (items.Count < amount) ? true : false,
                    Items = itemsMapped
                };
                return output;

            }
            catch(Exception ex) {
                return BadRequest("Error in parsing request");    
            }

        }
        
    }
}

