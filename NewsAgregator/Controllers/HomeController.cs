using Microsoft.AspNetCore.Mvc;
using NewsAgregator.Models;
using NewsAgregator.ViewModels;
using System.Diagnostics;
using NewsAgregator.Data;
using Microsoft.EntityFrameworkCore;

namespace NewsAgregator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomDbContext _db;
        public HomeController(ILogger<HomeController> logger, CustomDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new NewsInputModel();
            foreach (var source in _db.Sources.Include(p => p.Categories))
            {
                List<string> categories = new List<string>();
                source.Categories!.ForEach(p => categories.Add(p.Name));
                model.Sources.Add(new SourceInputModel
                {
                    Name = source.Name,
                    Categories = categories
                });
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}