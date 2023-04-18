using System.Text;

static class Transliterator
{
    static Dictionary<char, char> transliterationMap = new Dictionary<char, char>
    {
        { 'Є', 'Е' },
        { 'є', 'е' },
        { 'І', 'И' },
        { 'і', 'и' },
        { 'Ї', 'Й' },
        { 'ї', 'й' },
        { 'Ґ', 'Г' },
        { 'ґ', 'г' },
    };

    public static string Transliterate(string ukrainianText)
    {
        var russianText = new StringBuilder();

        foreach (var ukrainianChar in ukrainianText)
            russianText.Append(transliterationMap.ContainsKey(ukrainianChar) ? transliterationMap[ukrainianChar] : ukrainianChar);
        return russianText.ToString();
    }
}