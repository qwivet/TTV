using System.Text.RegularExpressions;

public class LanguageDetector
{
    private static readonly Regex EnglishRegex = new Regex(@"\p{IsBasicLatin}");
    private static readonly Regex UkrainianRegex = new Regex(@"\p{IsCyrillic}[єЄіІїЇґҐ]");
    private static readonly Regex RussianRegex = new Regex(@"\p{IsCyrillic}[ЁёЪъЫыЭэ]");

    public string Detect(string text)
    {
        var hasEnglish = EnglishRegex.IsMatch(text);
        var hasUkrainian = UkrainianRegex.IsMatch(text);
        var hasRussian = RussianRegex.IsMatch(text);

        if (hasEnglish && !hasUkrainian && !hasRussian) return "English";
        if (hasUkrainian) return "Ukrainian";
        if (hasRussian) return "Russian";
        return "Unknown";
    }
}