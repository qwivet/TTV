using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

public class Controller
{
    const string connectionString = "server=localhost;user=root;database=ttvbase;password=YmF6YWth";
    [HttpGet("/chat/name/{chatName}")]
    public long GetChat(string chatName)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"SELECT chatId FROM chats WHERE chatName = '{chatName}';";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        long chatId = 0;
        if (reader.Read()) 
            chatId = reader.GetInt64(reader.GetOrdinal("chatId")); 

        reader.Close();
        connection.Close();

        return chatId;
    }
    [HttpGet("/chat/{chatId:long}")]
    public string GetChat(long chatId)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"SELECT chatName FROM chats WHERE chatId = {chatId};";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        string chatName = null;
        if (reader.Read()) 
            chatName = reader.GetString(reader.GetOrdinal("chatName")); 

        reader.Close();
        connection.Close();

        return chatName;
    }

    [HttpPost("/chat")]
    public void PostChat([FromBody] Chat chat)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"INSERT INTO chats (chatId, chatName) VALUES ({chat.ChatId}, '{chat.ChatName}');";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();
        reader.Close();
        connection.Close();
    }
    [HttpGet("/user/name/{userName}")]
    public long GetUser(string userName)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"SELECT userId FROM users WHERE userName = '{userName}';";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        long chatId = 0;
        if (reader.Read()) 
            chatId = reader.GetInt64(reader.GetOrdinal("chatId")); 

        reader.Close();
        connection.Close();

        return chatId;
    }
    [HttpGet("/user/{userId:long}")]
    public string GetUser(long userId)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"SELECT userName FROM users WHERE userId = {userId};";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        string chatName = null;
        if (reader.Read()) 
            chatName = reader.GetString(reader.GetOrdinal("userName")); 

        reader.Close();
        connection.Close();

        return chatName;
    }

    [HttpPost("/user")]
    public void PostUser([FromBody] User user)
    {
        var connection = new MySqlConnection(connectionString);
        connection.Open();
        var sql = $"INSERT INTO users (userId, userName) VALUES ({user.UserId}, '{user.UserName}');";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();
        reader.Close();
        connection.Close();
    }
    [HttpPost("/gpt")]
    public async Task<Stream> GetGPTAnswer([FromBody]RText text)
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/chat/completions"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer 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"); 

                request.Content = new StringContent("{\n  \"model\": \"gpt-3.5-turbo\",\n  \"messages\": [{\"role\": \"user\", \"content\": \""+text.Text+"\"}]\n}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json"); 
                var response = await httpClient.SendAsync(request);
                var answer = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["choices"][0]["message"]["content"];
                if (new LanguageDetector().Detect(answer) == "ru-ru")
                    answer = Transliterator.Transliterate(answer);
                return await new TextToVoice().TextToSpeechAsync(answer, new LanguageDetector().Detect(answer));
            }
        }
    }
    [HttpPost("/gpt/concrete")]
    public async Task<Stream> GetGPTAnswerConcrete([FromBody]RText text)
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/chat/completions"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer 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"); 

                request.Content = new StringContent("{\n  \"model\": \"gpt-3.5-turbo\",\n  \"messages\": [{\"role\": \"user\", \"content\": \""+text.Text+"? Write your answer in English only. You can answer only 'Yes', 'No' or 'Maybe'. Try to use 'Maybe' as rarely as possible. For example, if you think, that chance that it is true bigger than chance that it is false, print 'Yes"+"\"}]\n}");
                
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json"); 
                var response = await httpClient.SendAsync(request);
                var answer = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["choices"][0]["message"]["content"];
                if (answer.ToLower().Contains("yes"))
                    return File.OpenRead(@"D:\Prog\1\TTV\TTVBot\yes.mp3");
                if (answer.ToLower().Contains("no"))
                    return File.OpenRead(@"D:\Prog\1\TTV\TTVBot\no.mp3");
                else
                    return File.OpenRead(@"D:\Prog\1\TTV\TTVBot\maybe.mp3");
            }
        }
    }
    [HttpPost("/ttv")]
    public async Task<Stream> GetTTV([FromBody]RText text)
    {
        var regex = new Regex(@"(-?[0-9]{1,2})x$");
        var match = regex.Match(text.Text);
        var speed = "0";
        if (match.Success)
        {
            var number = int.Parse(match.Groups[1].Value);
            if (number >= -10 && number <= 10)
                speed = number.ToString();
        }
        if (new LanguageDetector().Detect(text.Text) == "ru-ru")
            text.Text = Transliterator.Transliterate(text.Text);
        return await new TextToVoice().TextToSpeechAsync(text.Text, new LanguageDetector().Detect(text.Text), speed);
    }
}
public class RText
{
    public string Text { get; set; }
}

public class User
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}
public class Chat
{
    public long ChatId { get; set; }
    public string ChatName { get; set; }
}