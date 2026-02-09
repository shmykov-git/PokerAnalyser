namespace Model;

public static class ProbabilityExplainer
{
    private static readonly (double p, string exp)[] values = Enumerable.Range(1, 19).SelectMany(a => Enumerable.Range(1, 19).Select(b => (a, b)))
        .GroupBy(v => ((decimal)v.a / v.b))
        .Select(gv => new
        {
            p = (double)gv.Key,
            pair = gv.OrderBy(v => v.a).ThenBy(v => v.b).First(),
        })
        .OrderBy(v => v.p)
        .Select(v => (v.p, $"{v.pair.a}/{v.pair.b}"))
        .ToArray();

    private static readonly (double p, string exp)[] smallValues = Enumerable.Range(1, 9).SelectMany(a => Enumerable.Range(1, 9).Select(b => (a, b)))
        .GroupBy(v => ((decimal)v.a / v.b))
        .Select(gv => new
        {
            p = (double)gv.Key,
            pair = gv.OrderBy(v => v.a).ThenBy(v => v.b).First(),
        })
        .OrderBy(v => v.p)
        .Select(v => (v.p, $"{v.pair.a}/{v.pair.b}"))
        .ToArray();


    public static string ToExplanation(this double p)
    {
        var v = values.MinBy(v => Math.Abs(v.p - p) / v.p).exp;
        var sv = smallValues.MinBy(v => Math.Abs(v.p - p) / v.p).exp;

        return v == sv || v.StartsWith("1") ? v : $"{v} (~{sv})";
    }
}