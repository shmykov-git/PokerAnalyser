using Model;
using System.Diagnostics;
using System.Text.Json;

var rnd = new Random(0);

AnalyseTask[] analyseTasks = 
[
    //new AnalyseTask
    //{
    //    Case = GameCase.Flop,
    //    IteractionCount = 100_000,
    //    TableRange = (2, 10),
    //    CombinationFn = hand => hand.HasPair(),
    //    Description = "Any pair on flop"
    //},
    //new AnalyseTask
    //{
    //    Case = GameCase.Flop,
    //    IteractionCount = 100_000,
    //    TableRange = (2, 10),
    //    CombinationFn = hand => hand.HasPair(Cards.RankQ),
    //    Description = "Q-pair and higher on flop"
    //},
    //new AnalyseTask
    //{
    //    Case = GameCase.PreFlop,
    //    IteractionCount = 100_000,
    //    TableRange = (2, 10),
    //    CombinationFn = hand => hand.HasPair(Cards.RankJ),
    //    Description = "J-pair and higher preflop"
    //},
    //new AnalyseTask
    //{
    //    Case = GameCase.PreFlop,
    //    IteractionCount = 100_000,
    //    TableRange = (2, 10),
    //    CombinationFn = hand => hand.HasPair(Cards.RankA),
    //    Description = "A-pair preflop"
    //},
    new AnalyseTask
    {
        Case = GameCase.Flop,
        IteractionCount = 100_000,
        TableRange = (2, 10),
        CombinationFn = hand => hand.HasPair(Cards.RankA),
        Description = "A-pair on flop"
    },];

async Task<AnalyseResult> AnalyseByMonteCarlo(AnalyseTask task)
{
    var result = new AnalyseResult() { Description = task.Description };

    for (var n = task.TableRange.from; n <= task.TableRange.to; n++)
    {
        var tableResult = new AnalyseResult.Table { N = n };
        result.Tables.Add(tableResult);
        var counter = 0;

        for (var i = 0; i < task.IteractionCount; i++)
        {
            var deck = new Deck(rnd);
            var tableCards = deck.TakeTableCards(task.Case);

            for (var j = 0; j < n; j++)
            {
                var hand = deck.TakeHandWithTableCards(tableCards);

                if (task.CombinationFn(hand))
                {
                    counter++;
                    break;
                }
            }
        }

        tableResult.Probability = 1.0 * counter / task.IteractionCount;
    }

    return result;
}

var tasks = analyseTasks.Select(AnalyseByMonteCarlo).ToArray();
await Task.WhenAll(tasks);
var results = tasks.Select(t => t.Result).ToArray();

var json = JsonSerializer.Serialize(results, new JsonSerializerOptions
{
    WriteIndented = true
});

Debug.WriteLine(json);
File.WriteAllText("AnalyseResult.json", json);

