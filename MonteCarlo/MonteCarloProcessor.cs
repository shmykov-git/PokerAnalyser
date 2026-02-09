using Model;

namespace MonteCarlo;

public static class MonteCarloProcessor
{
    public static async Task<AnalyseResult[]> Analyse(AnalyseTask[] analyseTasks, int seed = 0, int? iterationCount = null)
    {
        var tasks = analyseTasks.Select(t => Analyse(t, seed, iterationCount)).ToArray();
        await Task.WhenAll(tasks);
        var results = tasks.Select(t => t.Result).ToArray();

        return results;
    }

    private static async Task<AnalyseResult> Analyse(AnalyseTask task, int seed, int? iterationCount)
    {
        var rnd = new Random(seed);
        var result = new AnalyseResult() { Description = task.Description };
        var iterCount = task.IterationCount ?? iterationCount ?? 100_000;

        for (var n = task.TableRange.from; n <= task.TableRange.to; n++)
        {
            var tableResult = new AnalyseResult.Table { N = n };
            result.Tables.Add(tableResult);
            var conditionCounter = 0;
            var combinationCounter = 0;

            for (var i = 0; i < iterCount; i++)
            {
                var deck = new Deck(rnd);

                var nSuit = Cards.suits[rnd.Next(4)];
                int nOponent = 1 + rnd.Next(n - 1);

                var hands = Enumerable.Range(0, n).Select(j =>
                    task.Case switch
                    {
                        TableCardsCase.OponentHasSameSuited =>
                            j == 0 || j == nOponent
                            ? deck.TakeSuitedHand(nSuit)
                            : deck.TakeHand(),
                        _ => deck.TakeHand()
                    }
                ).ToArray();

                var tableCards = deck.TakeTableCards(task.GameCase);
                var flop = tableCards.Length <= 3 ? tableCards : tableCards.Take(3).ToArray();
                var turn = tableCards.Length <= 3 ? [] : tableCards.Skip(3).Take(1).ToArray();

                var hasCondition = task.MyConditionFn(hands[0], flop, turn) && hands.Any(hand => task.ConditionFn(hand, flop, turn));

                if (!hasCondition)
                    continue;

                conditionCounter++;

                var fullHands = hands.Select(hand => hand.cards.Concat(tableCards).ToArray()).ToArray();

                if (task.MyCombinationFn(fullHands[0]) && fullHands.Any(h => task.CombinationFn(h)))
                { 
                    combinationCounter++;
                    continue;
                }
            }

            tableResult.Probability = 1.0 * combinationCounter / conditionCounter;
            tableResult.Explanation = tableResult.Probability.ToExplanation();
        }

        result.AverageTable = new AnalyseResult.Table
        {
            N = result.Tables.Average(t => t.N),
            Probability = result.Tables.Average(t => t.Probability),
            Explanation = result.Tables.Average(t => t.Probability).ToExplanation()
        };

        return result;
    }
}
