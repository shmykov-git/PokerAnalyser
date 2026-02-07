using Model;

namespace MonteCarlo;

public static class MonteCarloProcessor
{
    public static async Task<AnalyseResult> AnalyseByMonteCarlo(AnalyseTask task, Random rnd)
    {
        var result = new AnalyseResult() { Description = task.Description };

        for (var n = task.TableRange.from; n <= task.TableRange.to; n++)
        {
            var tableResult = new AnalyseResult.Table { N = n };
            result.Tables.Add(tableResult);
            var combinationCounter = 0;
            var caseCounter = 0;

            for (var i = 0; i < task.IterationCount; i++)
            {
                var deck = new Deck(rnd);
                var tableCards = deck.TakeTableCards(task.Case);

                var isCase = task.CaseConditionFn == null;

                for (var j = 0; j < n; j++)
                {
                    var hand = deck.TakeHand();

                    if (task.CaseConditionFn != null)
                        if (!task.CaseConditionFn(tableCards, hand))
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
        }

        return result;
    }
}
