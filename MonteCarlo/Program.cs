using Model;
using MonteCarlo;

var seed = 7;
var iterationCount = 100_000;

AnalyseTask[] analyseTasks =
[
    new AnalyseTask
    {
        Case = GameCase.PreFlop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankJ),
        Description = "J-pair and higher preflop"
    },
    new AnalyseTask
    {
        Case = GameCase.PreFlop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankA),
        Description = "A-pair preflop"
    },

    new AnalyseTask
    {
        Case = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(),
        Description = "Any pair on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankQ),
        Description = "Q-pair and higher on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasDoubleStraightDraw(),
        Description = "Double straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => flop.HasFlushPair(),
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasFlushPair(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on hand"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasConnectors(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop when connectors on hand"
    },
    new AnalyseTask
    {
        Case = GameCase.Flop,
        CaseConditionFn = (flop, hand) => hand.cards.HasConnectors() && hand.cards.HasFlushPair(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "Straight draw or flush draw on flop when same suit connectors on hand"
    },

    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasFlushDraw(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasFlush(),
        Description = "Flush on river when flush draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasDoubleStraightDraw(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when double straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasStraightDraw(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when straight draw on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        TableRange = (1, 10),
        CombinationFn = hand => hand.HasFourOfAKind(),
        Description = "Four of a kind on the river"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        TableRange = (1, 10),
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasSet(),
        CombinationFn = hand => hand.HasFourOfAKind(),
        Description = "Four of a kind on the river when set on flop"
    },
    new AnalyseTask
    {
        Case = GameCase.River,
        TableRange = (1, 10),
        CaseConditionFn = (flop, hand) => hand.AddCards(flop).HasSet(),
        CombinationFn = hand => hand.HasFullHouse(),
        Description = "Full house on the river when set on flop"
    },
];

var result = await MonteCarloProcessor.Analyse(analyseTasks, seed, iterationCount);
var json = result.ToJson();
File.WriteAllText("AnalyseResult.json", json);

