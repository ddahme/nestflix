using System.Text;
using System.Text.Json;
using Api.DTOs;
using OpenAI.Chat;

namespace Api.Services;

public interface IAiService
{
    Task<AIResponse> AnalyzeImageAsync(AIRequest request, CancellationToken cancellationToken = default);
}

public class AiService(ChatClient chatClient, ILogger<AiService> logger) : IAiService
{
    public async Task<AIResponse> AnalyzeImageAsync(AIRequest request, CancellationToken cancellationToken = default)
    {
        using MemoryStream ms = new();
        await request.Image.CopyToAsync(ms, cancellationToken);
        
        string imageAsBase64 = Convert.ToBase64String(ms.ToArray());
        
        List<ChatMessage> messages = [
            new SystemChatMessage("The user has provided you with an image of a bird box encoded in base64. Please extract the following information: type of bird, number of eggs, number of hatched chicks, and whether the bird box is occupied."),
            new SystemChatMessage("Please provide the extracted information in the following JSON format: { \"birdType\": \"\", \"eggCount\": 0, \"hatchedCount\": 0, \"isOccupied\": false }"),
            new UserChatMessage($"Image: {imageAsBase64}")
        ];
        ChatCompletionOptions options = new () {
            Temperature = (float)0.7,
            MaxOutputTokenCount = 4096,
            TopP=(float)0.95,
            FrequencyPenalty=(float)0,
            PresencePenalty=(float)0
        };
        
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options, cancellationToken);
        
        StringBuilder builder = new();
        foreach (ChatMessageContentPart message in completion.Content)
        {
            builder.AppendLine(message.Text);
        }
        
        AIResponse response = JsonSerializer.Deserialize<AIResponse>(builder.ToString(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}) ?? throw new JsonException();
        return response;
    }
}