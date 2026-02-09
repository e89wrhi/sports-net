using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.AI;
using Sport.Common.AI;
using Sport.Common.Core;
using System.Text.Json;

namespace Intelligence.Features.PredictMatch.V1;


internal class PredictMatchHandler : IRequestHandler<PredictMatchCommand, PredictMatchCommandResult>
{
    private readonly IPredict _predict;

    public PredictMatchHandler(IPredict predict)
    {
        _predict = predict;
    }

    public async Task<PredictMatchCommandResult> Handle(PredictMatchCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var ctx = request;

        var systemPrompt =
            """
            You are a professional sports match analyst.

            TASK:
            Predict the most likely match outcome based on:
            - Current score and minute
            - Match events
            - Team strength and form
            - Fan voting signals

            RULES:
            - Be objective and realistic
            - Do NOT mention data sources
            - Do NOT mention probabilities calculation logic
            - Output STRICT JSON in the following format:

            {
              "prediction": "HomeWin | Draw | AwayWin",
              "homeWinProbability": 0.00,
              "drawProbability": 0.00,
              "awayWinProbability": 0.00
            }

            Probabilities must sum to 1.0
            """
            ;

        var userPrompt = $"""
            MATCH:
            {ctx.HomeTeam} vs {ctx.AwayTeam}

            SCORE:
            {ctx.HomeScore} - {ctx.AwayScore} at minute {ctx.Minute}

            EVENTS:
            {string.Join("\n", ctx.Events.Select(e => $"{e.Minute}' {e.Team} {e.Type}"))}

            HOME TEAM INFO:
            Ranking: {ctx.HomeTeamInfo.Ranking}
            Form: {ctx.HomeTeamInfo.Form}

            AWAY TEAM INFO:
            Ranking: {ctx.AwayTeamInfo.Ranking}
            Form: {ctx.AwayTeamInfo.Form}

            FAN VOTES:
            Home Win: {ctx.Votes.HomeWinVotes}
            Draw: {ctx.Votes.DrawVotes}
            Away Win: {ctx.Votes.AwayWinVotes}
            """;

        var messages = new[]
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, userPrompt)
        };

        var response = await _predict.PredictAsync(messages, request.ModelId, cancellationToken);
        var json = response;

        // Safe JSON parsing
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        return new PredictMatchCommandResult(
            root.GetProperty("prediction").GetString()!,
            root.GetProperty("homeWinProbability").GetDouble(),
            root.GetProperty("drawProbability").GetDouble(),
            root.GetProperty("awayWinProbability").GetDouble(),
            request.ModelId ?? "unknown",
            ProviderName: "unknown");
    }
}
