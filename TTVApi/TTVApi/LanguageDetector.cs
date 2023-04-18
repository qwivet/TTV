using System.Text.RegularExpressions;

public class LanguageDetector
{
    private static readonly Regex EnglishRegex = new Regex(@"\p{IsBasicLatin}");
    private static readonly Regex UkrainianRegex = new Regex(@"\p{IsCyrillic}");

    public string Detect(string text)
    {
        var hasEnglish = EnglishRegex.IsMatch(text);
        var hasUkrainian = UkrainianRegex.IsMatch(text);

        if (hasUkrainian) return "ru-ru";
        if (hasEnglish) return "en-us";
        return "Unknown";
    }
}