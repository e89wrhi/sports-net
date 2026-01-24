using Intelligence.Intelligence.ValueObjects;
using Sport.Common.Core;

namespace Intelligence.Intelligence.Models;

public record MatchPredictionId
{
    public Guid Value { get; }
    private MatchPredictionId(Guid value) => Value = value;
    public static MatchPredictionId Of(Guid value) => new(value);
    public static implicit operator Guid(MatchPredictionId id) => id.Value;
}

public record MatchPrediction : Aggregate<MatchPredictionId>
{
    public Guid MatchId { get; private set; }
    public PredictionProbability Probabilities { get; private set; } = default!;
    public string PredictedScore { get; private set; } = default!;
    public double Confidence { get; private set; }
    public string ModelName { get; private set; } = default!;
    public DateTime CalculatedAt { get; private set; }

    public static MatchPrediction Create(
        Guid matchId, 
        PredictionProbability probabilities, 
        string predictedScore, 
        double confidence, 
        string modelName)
    {
        var prediction = new MatchPrediction
        {
            Id = MatchPredictionId.Of(Guid.NewGuid()),
            MatchId = matchId,
            Probabilities = probabilities,
            PredictedScore = predictedScore,
            Confidence = confidence,
            ModelName = modelName,
            CalculatedAt = DateTime.UtcNow,
            Version = 1
        };

        prediction.AddDomainEvent(new MatchPredictionGeneratedDomainEvent(prediction.Id, matchId));
        return prediction;
    }

    public void RefreshPrediction(PredictionProbability probabilities, string predictedScore, double confidence)
    {
        Probabilities = probabilities;
        PredictedScore = predictedScore;
        Confidence = confidence;
        CalculatedAt = DateTime.UtcNow;
        Version++;
        
        AddDomainEvent(new MatchPredictionRefreshedDomainEvent(Id));
    }
}

public record MatchPredictionGeneratedDomainEvent(MatchPredictionId Id, Guid MatchId) : IDomainEvent;
public record MatchPredictionRefreshedDomainEvent(MatchPredictionId Id) : IDomainEvent;
