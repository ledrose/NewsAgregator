using System.ComponentModel.DataAnnotations;

namespace NewsAgregator.Models
{
    public enum SourceTypes
    {
        Rss,
        Telegram
    }
    public class Source
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public SourceTypes Type { get; set; }

        public string RSSUrl { get; set; }
        [RequiredEither("RSSUrl")]
        public string TelegramChanelName { get; set; }
        public List<NewsItem>? NewsItems { get; set; }

    }
}

public class RequiredEither : ValidationAttribute
{
    private readonly string _comparisionProperty;

    public RequiredEither(string comparitionProperty)
    {
        _comparisionProperty= comparitionProperty;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var aliasName = (string)value;
        var property = validationContext.ObjectType.GetProperty(_comparisionProperty);
        if (property == null)
        {
            throw new ArgumentException("Property with this name not found");
        }
        var aboutValue = (string)property.GetValue(validationContext.ObjectInstance)!;
        var aliasBool = aliasName != null;
        var aboutBool = aboutValue != null;
        if (aboutBool == aliasBool) {
            return new ValidationResult(ErrorMessageString);
        }
        return ValidationResult.Success!;
    }
}
