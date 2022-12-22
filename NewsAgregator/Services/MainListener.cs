namespace NewsAgregator.Services
{
    public class MainListener : BackgroundService
    {
        private readonly RSSListener _rssListener;
        private readonly TelegramListener _telegramListener;
        public MainListener(IServiceProvider serviceProvider, RSSListener rssListener, TelegramListener telegramListener)
        {
            _rssListener = rssListener;
            _telegramListener = telegramListener;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rssListener.update();
                _telegramListener.update();
                await Task.Delay(new TimeSpan(0, 30, 0));
            }
        }
    }
}
