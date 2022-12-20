using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueProjectBack.Data;
using VueProjectBack.Models;
using Newtonsoft.Json.Linq;
using NewsAgregator.ViewModels;
using AutoMapper;

namespace VueProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private CustomDbContext _db;
        private IMapper _mapper;
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
                var items = _db.NewsItems
                    .Where(p => sources.Contains(p.SourceName))
                    .OrderByDescending(p => p.CreationDate).Skip(startIndex).Take(amount)
                    .ToList();
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

