using Model;
using MonteCarlo;
using System.Diagnostics;
using System.Text.Json;

var rnd = new Random(0);

AnalyseTask[] analyseTasks = 
[
    new AnalyseTask
    {
        Case = GameCase.PreFlop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankJ),
        Description = "J-pair and higher preflop"
    },
    new AnalyseTask
    {
        Case = GameCase.PreFlop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankA),
        Description = "A-pair preflop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(),
        Description = "Any pair on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankQ),
        Description = "Q-pair and higher on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasDoubleStraightDraw(),
        Description = "Double straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => flop.HasFlushPair(),
        IterationCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasFlushPair(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on hand"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasConnectors(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop when connectors on hand"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasConnectors() && hand.cards.HasFlushPair(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "Straight draw or flush draw on flop when same suit connectors on hand"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasFlushDraw(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasFlush(),
        Description = "Flush on river when flush draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasDoubleStraightDraw(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when double straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasStraightDraw(),
        IterationCount = 100_000,
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when straight draw on flop"
    }
];

var tasks = analyseTasks.Select(t => MonteCarloProcessor.AnalyseByMonteCarlo(t, rnd)).ToArray();
await Task.WhenAll(tasks);
var results = tasks.Select(t => t.Result).ToArray();

var json = JsonSerializer.Serialize(results, new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
});

//Debug.WriteLine(json);
File.WriteAllText("AnalyseResult.json", json);

