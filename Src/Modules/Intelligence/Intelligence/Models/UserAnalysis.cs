using Sport.Common.Core;

namespace Intelligence.Intelligence.Models;

public record UserAnalysisId
{
    public Guid Value { get; }
    private UserAnalysisId(Guid value) => Value = value;
    public static UserAnalysisId Of(Guid value) => new(value);
    public static implicit operator Guid(UserAnalysisId id) => id.Value;
}

public record UserAnalysis : Aggregate<UserAnalysisId>
{
    public Guid UserId { get; private set; }
    
    /// <summary>
    /// Vector embedding representing the user's sports interests (768-d typically for LLMs)
    /// Used for semantic search and high-accuracy recommendations.
    /// </summary>
    public float[] InterestEmbedding { get; private set; } = default!;
    
    /// <summary>
    /// AI-generated psychological profile or fan persona (e.g., "Hardcore Fan", "Casual Observer", "Underdog Supporter")
    /// </summary>
    public string PersonaTag { get; private set; } = default!;
    
    public DateTime LastAnalyzed { get; private set; }

    public static UserAnalysis Create(Guid userId, float[] embedding, string persona)
    {
        var analysis = new UserAnalysis
        {
            Id = UserAnalysisId.Of(Guid.NewGuid()),
            UserId = userId,
            InterestEmbedding = embedding,
            PersonaTag = persona,
            LastAnalyzed = DateTime.UtcNow,
            Version = 1
        };

        analysis.AddDomainEvent(new UserAnalysisUpdatedDomainEvent(analysis.Id, userId));
        return analysis;
    }

    public void UpdatePersona(string persona, float[] newEmbedding)
    {
        PersonaTag = persona;
        InterestEmbedding = newEmbedding;
        LastAnalyzed = DateTime.UtcNow;
        Version++;
    }
}

public record UserAnalysisUpdatedDomainEvent(UserAnalysisId Id, Guid UserId) : IDomainEvent;
