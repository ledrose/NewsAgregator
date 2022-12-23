using TL;

namespace NewsAgregator.Services
{
    public class MainListener : BackgroundService
    {
        private readonly RSSListener _rssListener;
        private readonly TelegramListener _telegramListener;
        public MainListener(RSSListener rssListener, TelegramListener telegramListener)
        {
            _rssListener = rssListener;
            _telegramListener = telegramListener;
        }
        public async Task Update()
        {
            await _rssListener.Update();
            await _telegramListener.Update();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _telegramListener.DoLogin("+79138749535");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Update();
                await Task.Delay(new TimeSpan(0, 30, 0));
            }
        }
    }
}
