using System;
using System.Linq;
using NTextCat;

public class TextLanguageDetectionUtil {
    private readonly RankedLanguageIdentifierFactory _factory;
    private readonly RankedLanguageIdentifier _identifier;

    public TextLanguageDetectionUtil(IConfiguration configuration)
    {
        _factory = new RankedLanguageIdentifierFactory();
        _identifier = _factory.Load(configuration["LanguageRankingProfilePath"]);
    }

    public LanguageInfo? DetectLanguageFromText(string text)
    {
        // Detect the language
        var languages = _identifier.Identify(text);

        // Get the most probable language
        var mostProbableLanguage = languages.FirstOrDefault();

        if(mostProbableLanguage == null)
            return null;

        return mostProbableLanguage.Item1;
    }
}