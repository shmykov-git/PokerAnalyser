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
            var combinationCounter = 0;
            var caseCounter = 0;

            for (var i = 0; i < iterCount; i++)
            {
                var deck = new Deck(rnd);
                var tableCards = deck.TakeTableCards(task.Case);
                var flop = tableCards.Length <= 3 ? tableCards : tableCards.Take(3).ToArray();
                var turn = tableCards.Length < 3 ? [] : tableCards.Skip(3).Take(1).ToArray();

                var isCase = task.CaseConditionFn == null;

                for (var j = 0; j < n; j++)
                {
                    var hand = deck.TakeHand();

                    if (task.CaseConditionFn != null)
                        if (!task.CaseConditionFn(hand, flop, turn))
                            continue;

                    isCase = true;
                    SortedHand fullHand = hand.cards.Concat(tableCards).ToArray();

                    if (task.CombinationFn(fullHand))
                    {
                        combinationCounter++;
                        break;
                    }
                }

                if (isCase)
                    caseCounter++;
            }

            tableResult.Probability = 1.0 * combinationCounter / caseCounter;
            tableResult.Explanation = tableResult.Probability.ToExplanation();
        }

        return result;
    }
}
