public class TextToVoice
{
    private const string ApiUrl = "https://voicerss-text-to-speech.p.rapidapi.com/";
    private const string ApiKey = "4f495f81a1e341eb8ee1233fa7c96fd8";
    private const string RapidApiKey = "b366cd1c35msh60405e8839ae254p19ad06jsn92666783b13a";
    private const string RapidApiHost = "voicerss-text-to-speech.p.rapidapi.com";

    public async Task<Stream> TextToSpeechAsync(string text, string lang, string speed = "0")
    {
        using var httpClient = new HttpClient();

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("src", text),
            new KeyValuePair<string, string>("hl", lang),
            new KeyValuePair<string, string>("r", speed),
            new KeyValuePair<string, string>("c", "mp3"),
            new KeyValuePair<string, string>("f", "8khz_8bit_mono"),
        });

        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", RapidApiKey);
        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", RapidApiHost);
        
        var response = await httpClient.PostAsync($"{ApiUrl}?key={ApiKey}", content);
        response.EnsureSuccessStatusCode();

        return response.Content.ReadAsStream();

    }
}