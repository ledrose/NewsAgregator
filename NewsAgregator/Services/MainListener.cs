namespace NewsAgregator.Services
{
    public class MainListener : BackgroundService
    {
        private RSSListener _rssListener;
        public MainListener(IServiceProvider serviceProvider, RSSListener rssListener)
        {
            _rssListener = rssListener;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rssListener.update();
                await Task.Delay(new TimeSpan(0, 30, 0));
            }
        }
    }
}
