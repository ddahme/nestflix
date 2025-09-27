using System.Text;
using System.Text.Json;
using Api.DTOs;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using OpenAI.Chat;

namespace Api.Services;

public interface IAiService
{
    Task<AIResponse> AnalyzeImageAsync(AIRequest request, CancellationToken cancellationToken = default);
}

public class AiService(CustomVisionPredictionClient predictionClient, ILogger<AiService> logger) : IAiService
{
    private static readonly Guid ProjectId = Guid.Parse("c87c5393-29e5-424f-b54e-be7d82a24943");
    private static readonly string PublishedModelName = "Iteration1";
    
    public async Task<AIResponse> AnalyzeImageAsync(AIRequest request, CancellationToken cancellationToken = default)
    {
        ImagePrediction result = await predictionClient.DetectImageAsync(ProjectId, PublishedModelName, request.Image, cancellationToken: cancellationToken);

        int eggCount = 0, chickCount = 0;
        string? birdType = null;
        
        foreach (var prediction in result.Predictions)
        {
            if(prediction.Probability < 0.9)continue;

            if (prediction.TagName.Equals("chick", StringComparison.InvariantCultureIgnoreCase))
            {
                chickCount++;
            }
            else if (prediction.TagName.Equals("egg", StringComparison.InvariantCultureIgnoreCase))
            {
                eggCount++;
            }
            else
            {
                birdType = prediction.TagName;
            }
        }

        return new AIResponse()
        {
            BirdType = birdType ?? "unknown",
            EggCount = eggCount,
            HatchedCount = chickCount,
            IsOccupied = eggCount > 0 || chickCount > 0 || birdType != null
        };
    }
}