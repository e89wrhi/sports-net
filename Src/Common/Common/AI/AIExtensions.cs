using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sport.Common.Web;

namespace Sport.Common.AI;

public static class AIExtensions
{
    public static IServiceCollection AddCustomAI(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<AIOptions>(nameof(AIOptions));
        
        // For now, if no client is configured, we can register a simple one or just leave it for the user to configure.
        // However, to make it work out of the box for the user, we might want to register a mock if in development.
        
        // Example: logic to switch between OpenAI, Ollama, etc.
        // if (options.ChatClient == "OpenAI") ...
       
        services.AddScoped<IPredict, Predict>();
        
        // Register a fake chat client if none is registered to avoid DI errors.
        // In a real app, this would be replaced by OpenAI/Ollama/AzureOpenAI registration.
        services.AddSingleton<IChatClient, FakeChatClient>();
        
        return services;
    }

    private class FakeChatClient : IChatClient
    {
        public void Dispose() { }
        
        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        Task<ChatResponse> IChatClient.GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ChatResponse(new ChatMessage(ChatRole.Assistant, "This is a predictd version of your text (Mock AI).")));
        }
    }
}
