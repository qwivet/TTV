using System.Text;

class Transliterator
{
    private static readonly Dictionary<char, string> TransliterationMap = new ()
    {
        {'А', "A"}, {'а', "a"},
        {'Б', "B"}, {'б', "b"},
        {'В', "W"}, {'в', "w"},
        {'Г', "G"}, {'г', "g"},
        {'Ґ', "G"}, {'ґ', "g"},
        {'Д', "D"}, {'д', "d"},
        {'Е', "E"}, {'е', "e"},
        {'Є', "JE"}, {'є', "je"},
        {'Ж', "Ż"}, {'ж', "ż"},
        {'З', "Z"}, {'з', "z"},
        {'И', "Y"}, {'и', "y"},
        {'І', "I"}, {'і', "i"},
        {'Ї', "JI"}, {'ї', "ji"},
        {'Й', "J"}, {'й', "j"},
        {'К', "K"}, {'к', "k"},
        {'Л', "L"}, {'л', "l"},
        {'М', "M"}, {'м', "m"},
        {'Н', "N"}, {'н', "n"},
        {'О', "O"}, {'о', "o"},
        {'П', "P"}, {'п', "p"},
        {'Р', "R"}, {'р', "r"},
        {'С', "S"}, {'с', "s"},
        {'Т', "T"}, {'т', "t"},
        {'У', "U"}, {'у', "u"},
        {'Ф', "F"}, {'ф', "f"},
        {'Х', "CH"}, {'х', "ch"},
        {'Ц', "C"}, {'ц', "c"},
        {'Ч', "CZ"}, {'ч', "cz"},
        {'Ш', "SZ"}, {'ш', "sz"},
        {'Щ', "SZCZ"}, {'щ', "szcz"},
        {'Ь', ""}, {'ь', ""},
        {'Ю', "JU"}, {'ю', "ju"},
        {'Я', "JA"}, {'я', "ja"},
    };

    public string Transliterate(string ukrainianText)
    {
        var result = new StringBuilder();
        foreach (var c in ukrainianText)
        {
            if (TransliterationMap.ContainsKey(c))
                result.Append(TransliterationMap[c]);
            else
                result.Append(c);
        }
        return result.ToString();
    }
}