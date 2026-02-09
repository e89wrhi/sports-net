using Microsoft.Extensions.AI;

namespace Sport.Common.AI;

public interface IPredict
{
    Task<string> PredictAsync(IEnumerable<ChatMessage> messages, string? model = null, CancellationToken cancellationToken = default);
}

public class Predict : IPredict
{
    private readonly IChatClient _chatClient;

    public Predict(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> PredictAsync(IEnumerable<ChatMessage> messages, string? model = null, CancellationToken cancellationToken = default)
    {
        var response = await _chatClient.GetResponseAsync(messages, cancellationToken: cancellationToken);
        
        return response.Messages[0].Text ?? string.Empty;
    }
}
