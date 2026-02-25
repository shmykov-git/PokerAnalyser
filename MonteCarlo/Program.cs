using Model;
using Model.Extensions;
using MonteCarlo;

var seed = 7;
var iterationCount = 1_000_000;


AnalyseTask[] preflopTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankJ),
        Description = "J-pair+ preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankA),
        Description = "A-pair preflop"
    },
];

AnalyseTask[] flopTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(),
        Description = "Any pair on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankQ),
        Description = "Q-pair and higher on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasDoubleStraightDraw(),
        Description = "Double straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        ConditionFn = (hand, flop, turn) => flop.HasFlushPair(),
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        ConditionFn = (hand, flop, turn) => hand.cards.HasFlushPair(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasFlushDraw(),
        Description = "Flush draw on flop when flush pair on hand"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        ConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw(),
        Description = "Straight draw on flop when connectors on hand"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        ConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors() && hand.cards.HasFlushPair(),
        TableRange = (1, 1),
        CombinationFn = hand => hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "Straight draw or flush draw on flop when same suit connectors on hand"
    },
];

AnalyseTask[] riverHandEstimationTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        ConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors(),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Stright on river when connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        ConditionFn = (hand, flop, turn) => hand.cards.HasHandSuitedConnectors(),
        CombinationFn = hand => hand.HasStraight() || hand.HasFlush(),
        Description = "Stright or flush on river when suited connectors preflop"
    },
];

AnalyseTask[] riverTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        CombinationFn = hand => hand.HasFourOfAKind(),
        Description = "Four of a kind on the river"
    },
];

AnalyseTask[] riverFlopTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasFlushDraw(),
        CombinationFn = hand => hand.HasFlush(),
        Description = "Flush on river when flush draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasDoubleStraightDraw(),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when double straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasStraightDraw(),
        CombinationFn = hand => hand.HasStraight(),
        Description = "Straight on river when straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasSet(),
        CombinationFn = hand => hand.HasFourOfAKind(),
        Description = "Four of a kind on the river when set on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasSet(),
        CombinationFn = hand => hand.HasFullHouse(),
        Description = "Full house on the river when set on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        ConditionFn = (hand, flop, turn) => hand.Join(flop).HasFlushDraw(),
        CombinationFn = hand => hand.HasFlush(),
        Description = "Flush on river when flush draw on turn"
    },
];

AnalyseTask[] flushSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasFlushPair(),
        MyCombinationFn = hand => hand.HasFlush(),
        Description = "My flush on the river when I have flush pair preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasFlushPair(),
        MyCombinationFn = hand => hand.HasFlushDraw(),
        Description = "My flush draw on flop when I have flush pair on flop"
    },
    new AnalyseTask // 6/17 (~1/3)
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasFlushDrawStrict(),
        MyCombinationFn = hand => hand.HasFlush(),
        Description = "My flush on river when I have flash draw on flop"
    },
    new AnalyseTask // 1/5
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop, turn).HasFlushDrawStrict(),
        MyCombinationFn = hand => hand.HasFlush(),
        Description = "My flush on river when I have flash draw on turn"
    },
    new AnalyseTask // 2/7
    {
        GameCase = GameCase.River,
        Case = TableCardsCase.OponentHasSameSuited,
        TableRange = (2, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasFlushDrawStrict(),
        MyCombinationFn = hand => hand.HasFlush(),
        Description = "My flush on river when one oponent has same suited flop"
    },
];

AnalyseTask[] staightSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors(),
        MyCombinationFn = hand => hand.HasStraight(),
        Description = "My straight on the river when I have connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors(),
        MyCombinationFn = hand => hand.HasStraightDraw(),
        Description = "My straight draw on flop when I have connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandConnectors(),
        MyCombinationFn = hand => hand.HasDoubleStraightDraw(),
        Description = "My double straight draw on flop when I have connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasStraightDrawStrict(),
        MyCombinationFn = hand => hand.HasStraight(),
        Description = "My straight on river when I have straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop, turn).HasStraightDrawStrict(),
        MyCombinationFn = hand => hand.HasStraight(),
        Description = "My straight on river when I have straight draw on turn"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasDoubleStraightDrawStrict(),
        MyCombinationFn = hand => hand.HasStraight(),
        Description = "My straight on river when I have double straight draw on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop, turn).HasDoubleStraightDrawStrict(),
        MyCombinationFn = hand => hand.HasStraight(),
        Description = "My straight on river when I have double straight draw on turn"
    },
];

AnalyseTask[] staightOrFlushSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandSuitedConnectors(),
        MyCombinationFn = hand => hand.HasStraight() || hand.HasFlush(),
        Description = "My straight or flush on the river when I have suited connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandSuitedConnectors(),
        MyCombinationFn = hand => hand.HasDoubleStraightDraw() || hand.HasFlushDraw(),
        Description = "My double straight or flush draw on flop when I have connectors preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandSuitedConnectors(),
        MyCombinationFn = hand => hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "My straight or flush draw on flop when I have connectors preflop"
    },
];

AnalyseTask[] setSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandPair(),
        MyCombinationFn = hand => hand.HasSet(),
        Description = "My set+ on the river when I have pair preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandPair(),
        MyCombinationFn = hand => hand.HasSet(),
        Description = "My set on flop when I have pair preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasSetStrict(),
        MyCombinationFn = hand => hand.HasFullHouse(),
        Description = "My full house on river when I have set on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop, turn).HasSetStrict(),
        MyCombinationFn = hand => hand.HasFullHouse(),
        Description = "My full house on river when I have set on turn"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasSetStrict(),
        MyCombinationFn = hand => hand.HasFourOfAKind(),
        Description = "My four of a kind on river when I have set on flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop, turn).HasSetStrict(),
        MyCombinationFn = hand => hand.HasFourOfAKind(),
        Description = "My four of a kind on river when I have set on turn"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasPairStrict(),
        MyCombinationFn = hand => hand.HasSet(),
        Description = "My set+ on river when I still have only pair flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasTwoPairsStrict(),
        MyCombinationFn = hand => hand.HasSet(),
        Description = "My set+ on river when I still have two pairs flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        CombinationFn = hand => hand.HasPair(),
        Description = "Somebody has pair preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (2, 10),
        MyConditionFn = (hand, flop, turn) => hand.cards.HasHandPair(),
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasPair()),
        Description = "Somebody also has pair preflop when I have pair preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (2, 10),
        MyConditionFn = (hand, flop, turn) => hand.Join(flop).HasSetStrict(),
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasSet()),
        Description = "Somebody also has set flop when I have set flop"
    },
];

AnalyseTask[] pairSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (2, 10),
        MyHand = [('J', '♥'), ('J', '♦')],
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasPair(Cards.RankQ)),
        Description = "Somebody has AA, KK or QQ preflop when I have JJ preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (2, 10),
        MyHand = [('A', '♥'), ('2', '♦')],
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasCard(Cards.RankA)),
        Description = "Somebody has A? preflop when I have A? preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (2, 10),
        MyHand = [('A', '♥'), ('J', '♦')],
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasAHigh(Cards.RankQ)),
        Description = "Somebody has AA, AK, AQ preflop when I have AJ preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (2, 10),
        MyHand = [('A', '♥'), ('J', '♦')],
        CombinationsFn = hands => hands.Skip(1).Any(h => h.HasAHigh(Cards.RankQ) || h.HasPair(Cards.RankJ)),
        Description = "Somebody has AA, AK, AQ, KK, QQ, JJ preflop when I have AJ preflop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.River,
        TableRange = (1, 10),
        MyHand = ["A♥", "2♦"],
        MyCombinationFn = hand => hand.HasPair(Cards.RankA),
        Description = "I have AA on the river when I have A? preflop"
    },
];

AnalyseTask[] handSecretTasks =
[
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        MyCombinationFn = hand => hand.HasHandRankPair(["AA", "KK", "QQ", "AK"]),
        Description = "I have AA, KK, QQ, AK on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        MyCombinationFn = hand => hand.HasHandRankPair(["AA", "KK", "QQ", "JJ", "TT", "AK", "AQ"]),
        Description = "I have AA, KK, QQ, JJ, TT, AK, AQ on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.PreFlop,
        TableRange = (1, 10),
        CombinationsFn = hands => hands.Any(h=>h[0].rank == Cards.RankA),
        Description = "Somebody has A"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["A♥", "Q♥"],
        MyCombinationFn = hand => hand.HasPair(),
        Description = "I have pair on flop when I have no preflop pair"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["A♥", "Q♥"],
        MyConditionFn = (hand, flop, _) => hand.Join(flop).HasTwoPairsStrict(),
        OtherCombinationsFn = hands => hands.Any(hand => hand.HasTwoPairs() || hand.HasStraight() || hand.HasSet() || hand.HasFlush()),
        Description = "Somebody has two pair or hight when I have only pair on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["5♥", "6♥"],
        MyCombinationFn = hand => hand.HasPair() || hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "I have suited connectors preflop. My continuation probablities on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["5♥", "6♥"],
        MyCombinationFn = hand => hand.HasPair(),
        Description = "I have suited connectors preflop. My pair continuation probablities on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["5♥", "6♥"],
        MyCombinationFn = hand => hand.HasStraightDraw() || hand.HasFlushDraw(),
        Description = "I have suited connectors preflop. My strong continuation probablities on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["5♥", "8♥"],
        MyCombinationFn = hand => hand.HasSet(),
        Description = "I have any hand preflop and set on the flop"
    },
    new AnalyseTask
    {
        GameCase = GameCase.Flop,
        TableRange = (1, 10),
        MyHand = ["5♥", "6♦"],
        MyCombinationFn = hand => hand.HasSet() || hand.HasStraight() || hand.HasTwoPairs(),
        Description = "I have connectors preflop and good posion on the flop"
    },
];

AnalyseTask[] analyseTasks = new[]
{
    //preflopTasks,
    //flopTasks,
    //riverTasks,
    //riverFlopTasks,
    //flushSecretTasks,
    //staightSecretTasks,
    //staightOrFlushSecretTasks,
    //setSecretTasks,
    //pairSecretTasks,
    handSecretTasks
}.SelectMany(v => v).ToArray();




var result = await MonteCarloProcessor.Analyse(analyseTasks, seed, iterationCount);
var json = result.ToJson();
File.WriteAllText("AnalyseResult.json", json);

