using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAgregator.Data;
using NewsAgregator.Models;
using NewsAgregator.Services;

namespace NewsAgregator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SourcesController : Controller
    {
        private readonly CustomDbContext _db;
        private readonly RSSListener _rssListener;
        private readonly TelegramListener _telegramListener;

        public SourcesController(CustomDbContext context, RSSListener rssListener, TelegramListener telegramListener)
        {
            _db = context;
            _rssListener = rssListener;
            _telegramListener = telegramListener;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _db.Sources.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Source source)
        {
            if (ModelState.IsValid)
            {
                _db.Add(source);
                _db.SaveChanges();
                if (source.Type==SourceType.Rss)
                {
                    await _rssListener.Update();
                }
                if (source.Type == SourceType.Telegram)
                {
                    await _telegramListener.Update();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(source);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _db.Sources == null)
            {
                return NotFound();
            }

            var source = await _db.Sources.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }
            return View(source);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Source source)
        {
            if (ModelState.IsValid)
            {
                _db.Update(source);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(source);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _db.Sources == null)
            {
                return NotFound();
            }

            var source = await _db.Sources
                .FirstOrDefaultAsync(m => m.Name == id);
            if (source == null)
            {
                return NotFound();
            }

            return View(source);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_db.Sources == null)
            {
                return Problem("Entity set 'CustomDbContext.Sources'  is null.");
            }
            var source = _db.Sources.Find(id);
            if (source != null)
            {
                _db.Sources.Remove(source);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceExists(string id)
        {
          return _db.Sources.Any(e => e.Name == id);
        }
    }
}
