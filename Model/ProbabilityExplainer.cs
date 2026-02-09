namespace Model;

public static class ProbabilityExplainer
{
    private static readonly (double p, string exp)[] values =
    [
        (1, "100%"),
        (0.9722, "Roll 3+ on 2d6"),
        (0.9167, "Roll 4+ on 2d6"),
        (0.8333, "Roll 2+ on 1d6"),
        (0.7222, "Roll 6+ on 2d6"),
        (0.6667, "Two thirds"),
        (0.5833, "Roll 7+ on 2d6"),
        (0.5000, "Heads or Tails"),
        (0.4167, "Roll 8+ on 2d6"),
        (0.3333, "One third"),
        (0.2778, "Roll 9+ on 2d6"),
        (0.1667, "Roll 6 on 1d6"),
        (0.0833, "Roll 11+ on 2d6"),
        (0.0278, "Roll 12 on 2d6"),
        (0.01, "One in a hundred"),
        (0.001, "One in a thousand"),
    ];

    public static string ToExplanation(this double p) => values.MinBy(v => Math.Abs(v.p - p) / v.p).exp;
}