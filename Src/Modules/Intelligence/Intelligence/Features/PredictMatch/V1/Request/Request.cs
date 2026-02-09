namespace Intelligence.Features.PredictMatch.V1;

public record PredictMatchRequestDto(Guid MatchId, string? ModelId = null);
public record PredictMatchResponseDto(string Prediction,
    double HomeWinProbability,
    double DrawProbability,
    double AwayWinProbability, 
    string ModelId, 
    string? ProviderName);
