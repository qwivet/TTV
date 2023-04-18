using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NAudio.Wave;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

public class Commands
{
    private const string Path = @"C:\Users\User\Desktop\Prog\1\i\TTVBot\output.mp3";
    private const string PathSamples = @"C:\Users\User\Desktop\Prog\1\i\TTVBot\Samples\";
    private static HttpClient HttpClient = new HttpClient();
    private const string ApiUrl = "http://localhost:5224";
    private static TelegramBotClient? _client;

    private readonly Dictionary<string, Func<string[], Update, CancellationToken, Task>> Command = new()
        { { "/ttv", TextToVoice },{"/ask", Ask},{"/cask", CAsk},{"/chat", Chat},{"/name", Name},};

    public Commands(TelegramBotClient? client)
    {
        _client = client;
    }
    public async void Execute(Update update, CancellationToken cancellationToken)
    {
        var tokens = update.Message?.Text?.Split(' ');
        if (tokens?[0] is not ("/chat" or "/name"))
        {
            var request = HttpClient.GetAsync($"http://localhost:5224/chat/{update.Message.Chat.Id}");
            if (await request.Result.Content.ReadAsStringAsync() == "")
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "This chat haven't name. Add name using /chat *name*",
                    cancellationToken: cancellationToken);
                return;
            }
            request = HttpClient.GetAsync($"http://localhost:5224/user/{update.Message.From.Id}");
            if (await request.Result.Content.ReadAsStringAsync() == "")
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "You haven't nickname. Add name using /name *name*",
                    cancellationToken: cancellationToken);
                return;
            }
        }

        if (Command.ContainsKey(tokens[0])) Command[tokens[0]](tokens[1..], update, cancellationToken);
        if (Command.ContainsKey(tokens[0].Length>16 ? tokens[0][..(tokens[0].Length-16)]:"")) Command[tokens[0][..(tokens[0].Length-16)]](tokens[1..], update, cancellationToken);
    }

    private static async Task Chat(string[] tokens, Update update, CancellationToken cancellationToken)
    {
        var name = tokens.Aggregate((x, y) => x + " " + y);
        var request = HttpClient.GetAsync($"http://localhost:5224/chat/name/{name}");
        if (name.Length > 255)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "This name is too long",
                cancellationToken: cancellationToken);
            return;
        }
        if (await request.Result.Content.ReadAsStringAsync() != "0")
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "This name is already used",
                cancellationToken: cancellationToken);
            return;
        }
        request = HttpClient.GetAsync($"http://localhost:5224/chat/{update.Message.Chat.Id}");
        if (await request.Result.Content.ReadAsStringAsync() != "")
        {
            HttpClient.PostAsync($"http://localhost:5224/chat", new StringContent(JsonSerializer.Serialize(new {ChatId = update.Message.Chat.Id, ChatName = name}), Encoding.Unicode, "application/json"));
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Chat renamed successfully",
                cancellationToken: cancellationToken);
            return;
        }

        HttpClient.PostAsync($"http://localhost:5224/chat", new StringContent(JsonSerializer.Serialize(new {ChatId = update.Message.Chat.Id, ChatName = name}), Encoding.Unicode, "application/json"));
        await _client.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Chat named successfully",
            cancellationToken: cancellationToken);
    }
    private static async Task Name(string[] tokens, Update update, CancellationToken cancellationToken)
    {
        var name = tokens.Aggregate((x, y) => x + " " + y);
        if (name.Length > 255)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "This name is too long",
                cancellationToken: cancellationToken);
            return;
        }
        var request = HttpClient.GetAsync($"http://localhost:5224/user/name/{name}");
        if (name.Length > 255)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "This name is too long",
                cancellationToken: cancellationToken);
            return;
        }
        if (await request.Result.Content.ReadAsStringAsync() != "0")
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "This name is already used",
                cancellationToken: cancellationToken);
            return;
        }
        request = HttpClient.GetAsync($"http://localhost:5224/user/{update.Message.Chat.Id}");
        if (await request.Result.Content.ReadAsStringAsync() != "")
        {
            HttpClient.PostAsync($"http://localhost:5224/user", new StringContent(JsonSerializer.Serialize(new {UserId = update.Message.From.Id, UserName = name}), Encoding.Unicode, "application/json"));
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Nickname changed successfully",
                cancellationToken: cancellationToken);
            return;
        }

        HttpClient.PostAsync($"http://localhost:5224/user", new StringContent(JsonSerializer.Serialize(new {UserId = update.Message.From.Id, UserName = name}), Encoding.Unicode, "application/json"));
        await _client.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Nickname setted successfully",
            cancellationToken: cancellationToken);
    }
    private static async Task Ask(string[] tokens, Update update, CancellationToken cancellationToken)
    {
        var text = tokens.Aggregate((x, y) => x + " " + y);
        var response = await HttpClient.PostAsync(ApiUrl + "/gpt", new StringContent(JsonSerializer.Serialize(new{Text=text}), Encoding.Unicode, "application/json"));

        var file = new InputFile(response.Content.ReadAsStream(), "output");
        await _client.SendVoiceAsync(
            chatId: update.Message.Chat.Id,
            voice: file,
            duration: 0,
            cancellationToken: cancellationToken);
    }
    private static async Task CAsk(string[] tokens, Update update, CancellationToken cancellationToken)
    {
        var text = tokens.Aggregate((x, y) => x + " " + y);
        var response = await HttpClient.PostAsync(ApiUrl + "/gpt/concrete", new StringContent(JsonSerializer.Serialize(new{Text=text}), Encoding.Unicode, "application/json"));

        var file = new InputFile(response.Content.ReadAsStream(), "output");
        await _client.SendVoiceAsync(
            chatId: update.Message.Chat.Id,
            voice: file,
            duration: 0,
            cancellationToken: cancellationToken);
    }
    private static async Task TextToVoice(string[] tokens, Update update, CancellationToken cancellationToken)
    {
        var text = tokens.Aggregate((x, y) => x + " " + y);
        var response = await HttpClient.PostAsync(ApiUrl + "/ttv", new StringContent(JsonSerializer.Serialize(new{Text=text}), Encoding.Unicode, "application/json"));

        var file = new InputFile(response.Content.ReadAsStream(), "output");
        await _client.SendVoiceAsync(
            chatId: update.Message.Chat.Id,
            voice: file,
            duration: 0,
            cancellationToken: cancellationToken);
    }

    private static TimeSpan GetDuration(string filePath)
    {
        using var mp3FileReader = new Mp3FileReader(filePath);
        return mp3FileReader.TotalTime;
    }
}