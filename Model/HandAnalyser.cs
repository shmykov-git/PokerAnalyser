using System.Diagnostics;

namespace Model;

public static class HandAnalyser
{
    public static Combination ToCombination(this SortedHand hand)
    {
        if (hand.HasStraightFlush())
            return Combination.StrightFlush;

        if (hand.HasFourOfAKind())
            return Combination.FourOfAKind;

        if (hand.HasFullHouse())
            return Combination.FullHouse;

        if (hand.HasFlush())
            return Combination.Flush;

        if (hand.HasStraight())
            return Combination.Stright;

        if (hand.HasSet())
            return Combination.Set;

        if (hand.HasTwoPairs())
            return Combination.TwoPairs;

        if (hand.HasPair())
            return Combination.Pair;

        return Combination.HighCard;
    }

    private static void MoveUp(Card[] cs, int i, int pos)
    {
        var c = cs[i];

        for (var j = i; j > pos; j--)
            cs[j] = cs[j - 1];

        cs[pos] = c;
    }

    private static Card[] GetHighCardWinHand(this SortedHand hand)
    {
        return hand.cards.Take(5).ToArray();
    }

    private static IEnumerable<int> GetIndices(Card[] hand, Func<Card, bool> condition)
    {
        foreach (var card in hand.Select((v, i) => (v, i)))
            if (condition(card.v))
                yield return card.i;
    }

    private static IEnumerable<int> GetCirclePairIndices(Card[] hand, Func<Card, Card, bool> condition)
    {
        for (var i = 0; i < hand.Length-1; i++) 
            if (condition(hand[i], hand[i+1]))
                yield return i;

        if (condition(hand[hand.Length - 1], hand[0]))
            yield return hand.Length - 1;
    }

    private static void MoveUp(Card[] hand, Func<Card, bool> condition, int from = 0)
    {
        var indices = GetIndices(hand, condition).ToArray();

        foreach (var (i, pos) in indices.Select((i, pos) => (i, pos)))
            MoveUp(hand, i, pos + from);
    }

    private static Card[] GetPairWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();
        
        var r = wHand
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 2)
            .Select(gc => gc.Key)
            .Single();

        MoveUp(wHand, c => c.rank == r);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetTwoPairsWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var r21 = wHand
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 2)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r21);

        var r22 = wHand
            .Skip(2)
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 2)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r22, 2);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetSetWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var r = wHand
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 3)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetStrightWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var j = -1;
        var indices = new List<int>();

        foreach(var i in GetCirclePairIndices(wHand, (a, b) => a.rank == b.rank + 1 || (a.rank == 2 && b.rank == 14)))
        {
            if (i == j)
            {
                indices.Add(i);
                j++;
            }
            else
            {
                indices = new List<int>([i]);
                j = i + 1;
            }
        }

        if (indices.Count < 4)
            throw new Exception("Not a stright");

        if (indices[^1] != 6)
            indices.Add(indices[^1] + 1);

        foreach (var (i, pos) in indices.Select((i, pos) => (i, pos)))
            MoveUp(wHand, i, pos);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetFlushWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var fS = wHand.GroupBy(c => c.suit).MaxBy(gs => gs.Count())!.First().suit;
        MoveUp(wHand, c => c.suit == fS);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetFullHouseWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var r3 = wHand
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 3)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r3);

        var r2 = wHand
            .Skip(3)
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() >= 2)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r2, 3);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetFourOfAKindWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var r = wHand
            .GroupBy(c => c.rank)
            .Where(gc => gc.Count() == 4)
            .Select(gc => gc.Key)
            .MaxBy(r => r);

        MoveUp(wHand, c => c.rank == r);

        return wHand.Take(5).ToArray();
    }

    private static Card[] GetStrightFlushWinHand(this SortedHand hand)
    {
        var wHand = hand.cards.ToArray();

        var j = -1;
        var indices = new List<int>();

        foreach (var i in GetCirclePairIndices(wHand, (a, b) => (a.rank == b.rank + 1 || (a.rank == 2 && b.rank == 14)) && a.suit == b.suit))
        {
            if (i == j)
            {
                indices.Add(i);
                j++;
            }
            else
            {
                indices = new List<int>([i]);
                j = i + 1;
            }
        }

        if (indices.Count < 4)
            throw new Exception("Not a stright flush");

        if (indices[^1] != 6)
            indices.Add(indices[^1] + 1);

        foreach (var (i, pos) in indices.Select((i, pos) => (i, pos)))
            MoveUp(wHand, i, pos);

        return wHand.Take(5).ToArray();
    }

    public static Card[] GetWinHand(this SortedHand hand, Combination combination)
    {
        if (hand.Count != 7)
            throw new ArgumentException("Win hand must be full hand of 7 cards");

        return combination switch
        {
            Combination.HighCard => GetHighCardWinHand(hand),
            Combination.Pair => GetPairWinHand(hand),
            Combination.TwoPairs => GetTwoPairsWinHand(hand),
            Combination.Set => GetSetWinHand(hand),
            Combination.Stright => GetStrightWinHand(hand),
            Combination.Flush => GetFlushWinHand(hand),
            Combination.FullHouse => GetFullHouseWinHand(hand),
            Combination.FourOfAKind => GetFourOfAKindWinHand(hand),
            Combination.StrightFlush => GetStrightFlushWinHand(hand),
            _ => throw new NotImplementedException(combination.ToString())
        };
    }

    public static Dictionary<(Combination, Combination), (int, int)> WinStats = new();

    public static int Win(this SortedHand myHand, SortedHand openentHand)
    {
        if (openentHand.Count != 7 || myHand.Count != 7)
            throw new ArgumentException("Invalid hands. Must be full hand of 7 cards");

        var a = myHand.ToCombination();
        var b = openentHand.ToCombination();

        if (a == b)
        {
            var aWin = GetWinHand(myHand, a);
            var bWin = GetWinHand(openentHand, b);

            for (var i = 0; i < 5; i++)
                if (aWin[i].rank == bWin[i].rank)
                    continue;
                else
                {
                    var sign = aWin[i].rank > bWin[i].rank ? 1 : -1;

                    //Debug.WriteLine($"{string.Join(' ', aWin)} {(sign == 1 ? '>' : '<')} {string.Join(' ', bWin)} {a}");

                    if (WinStats.ContainsKey((a, b)))
                        WinStats[(a, b)] = sign == 1 ? (WinStats[(a, b)].Item1 + 1, WinStats[(a, b)].Item2) : (WinStats[(a, b)].Item1, WinStats[(a, b)].Item2 + 1);
                    else
                        WinStats[(a, b)] = sign == 1 ? (1, 0) : (0, 1);

                    return sign;
                }

            return 0;
        }
        else
        {
            var sign = a > b ? 1 : -1;

            if (WinStats.ContainsKey((a, b)))
                WinStats[(a, b)] = sign == 1 ? (WinStats[(a, b)].Item1 + 1, WinStats[(a, b)].Item2) : (WinStats[(a, b)].Item1, WinStats[(a, b)].Item2 + 1);
            else
                WinStats[(a, b)] = sign == 1 ? (1, 0) : (0, 1);

            return sign;
        }
    }
}
