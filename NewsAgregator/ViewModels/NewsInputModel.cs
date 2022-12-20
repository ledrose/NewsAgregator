namespace NewsAgregator.ViewModels
{
    public class NewsInputModel
    {
        public int Amount { get; set; }
        public int StartIndex { get; set; }
        public IEnumerable<String> Sources { get; set; } = new List<String>();
    }
}
