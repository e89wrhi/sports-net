namespace Intelligence.Intelligence.ValueObjects;

public record PredictionProbability
{
    public double Win { get; }
    public double Draw { get; }
    public double Loss { get; }

    private PredictionProbability(double win, double draw, double loss)
    {
        Win = win;
        Draw = draw;
        Loss = loss;
    }

    public static PredictionProbability Of(double win, double draw, double loss)
    {
        if (win < 0 || draw < 0 || loss < 0)
            throw new ArgumentException("Probabilities cannot be negative.");

        var total = win + draw + loss;
        if (Math.Abs(total - 1.0) > 0.001)
            throw new ArgumentException("Probabilities must sum to 1.0.");

        return new PredictionProbability(win, draw, loss);
    }
}
