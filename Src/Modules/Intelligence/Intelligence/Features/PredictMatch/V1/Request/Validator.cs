using FluentValidation;

namespace Intelligence.Features.PredictMatch.V1;

public class PredictMatchWithAICommandValidator : AbstractValidator<PredictMatchCommand>
{
    public PredictMatchWithAICommandValidator()
    {
        RuleFor(x => x.HomeTeam).NotEmpty();
        RuleFor(x => x.AwayTeamInfo).NotEmpty();
    }
}
