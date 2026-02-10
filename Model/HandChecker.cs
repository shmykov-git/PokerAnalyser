namespace Model;

public static class HandChecker
{
    public static bool HasPairStrict(this SortedHand hand) => hand.HasPair() && !hand.HasSet() && !hand.HasTwoPairs() && !hand.HasFullHouse() && !hand.HasFourOfAKind();

    public static bool HasPair(this SortedHand hand, int minRank = 2)
    {
        for (var i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].rank < minRank)
                return false;

            if (hand[i].rank == hand[i + 1].rank)
                return true;
        }

        return false;
    }

    public static bool HasTwoPairsStrict(this SortedHand hand) => hand.HasTwoPairs() && !hand.HasSet() && !hand.HasFullHouse() && !hand.HasFourOfAKind();

    public static bool HasTwoPairs(this SortedHand hand, int minRank = 2)
    {
        var firstPairRank = -1;

        for (var i = 0; i < hand.Count - 1; i++)
        {
            if (firstPairRank == -1 && hand[i].rank < minRank)
                return false;

            if (hand[i].rank == hand[i + 1].rank)
            {
                if (firstPairRank == -1)
                    firstPairRank = hand[i].rank;
                else if (hand[i].rank != firstPairRank)
                    return true;
            }
        }

        return false;
    }

    public static bool HasSetStrict(this SortedHand hand) => hand.HasSet() && !hand.HasFourOfAKind() && !hand.HasFullHouse();

    public static bool HasSet(this SortedHand hand)
    {
        for (var i = 0; i < hand.Count - 2; i++)
        {
            if (hand[i].rank == hand[i + 1].rank && hand[i].rank == hand[i + 2].rank)
                return true;
        }

        return false;
    }

    public static bool HasFourOfAKind(this SortedHand hand)
    {
        for (var i = 0; i < hand.Count - 3; i++)
        {
            if (hand[i].rank == hand[i + 1].rank && hand[i].rank == hand[i + 2].rank && hand[i].rank == hand[i + 3].rank)
                return true;
        }

        return false;
    }

    public static bool HasStraight(this SortedHand hand)
    {
        var count = 0;

        for (var i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].rank == hand[i + 1].rank + 1)
                count++;
            else
                count = 0;

            if (count == 4)
                return true;
        }

        if (hand.LastCard.rank == 2 && hand.FirstCard.rank == 14)
            count++;

        if (count == 4)
            return true;

        return false;
    }

    public static bool HasStraightDrawStrict(this SortedHand hand) => hand.HasStraightDraw() && !hand.HasDoubleStraightDraw() && !hand.HasStraight();
    public static bool HasStraightDrawStrictD(this SortedHand hand) => hand.HasStraightDraw() && !hand.HasStraight();

    public static bool HasStraightDraw(this SortedHand hand)
    {
        var count = 0;
        var strights = new List<(int i, int count)>();

        for (var i = 0; i < hand.Count; i++)
        {
            if (hand[i].rank == hand.GetStrightRank(i + 1) + 1)
                count++;
            else if (count > 0)
            {
                strights.Add((i, count));
                count = 0;
            }
        }

        if (count > 0)
            strights.Add((hand.Count, count));

        if (strights.Count == 0)
            return false;

        for (var i = 0; i < strights.Count; i++)
        {
            if (strights[i].count >= 3)
                return true;

            if (strights[i].count == 2)
            {
                var (a, b) = (strights[i].i - 2, strights[i].i);

                if (a > 0 && 
                    hand[a].rank != Cards.RankA && 
                    hand[a-1].rank == hand[a].rank + 2)
                    return true;

                if (b < hand.Count - 1 &&
                    hand[b].rank == hand.GetStrightRank(b+1) + 2)
                    return true;
            }
        }

        // strights[*].count == 1
        for (var i = 0; i < strights.Count - 1; i++)
        {
            var (a, b) = (strights[i].i, strights[i + 1].i - 1);

            if (hand[a].rank == hand[b].rank + 2)
                return true;
        }

        return false;
    }

    public static bool HasDoubleStraightDrawStrict(this SortedHand hand) => hand.HasDoubleStraightDraw() && !hand.HasStraight();

    public static bool HasDoubleStraightDraw(this SortedHand hand)
    {
        var count = 0;
        var strights = new List<(int i, int count)>();

        for (var i = 0; i < hand.Count; i++)
        {
            if (hand[i].rank == hand.GetStrightRank(i + 1) + 1)
                count++;
            else if (count > 0)
            {
                strights.Add((i, count));
                count = 0;
            }
        }

        if (count > 0)
            strights.Add((hand.Count, count));

        if (strights.Count == 0)
            return false;

        for (var i = 0; i < strights.Count; i++)
        {
            if (strights[i].count >= 4)
                return true;

            if (strights[i].count == 3)
                return hand[strights[i].i - 3].rank != Cards.RankA && hand.GetStrightRank(strights[i].i) != Cards.SmallRankA;

            if (strights[i].count == 2)
            {
                var (a, b) = (strights[i].i - 2, strights[i].i);
                var connectorCount = 0;

                if (a > 0 &&
                    hand[a].rank != Cards.RankA &&
                    hand[a - 1].rank == hand[a].rank + 2)
                    connectorCount++;

                if (b < hand.Count - 1 &&
                    hand.GetStrightRank(b) != Cards.SmallRankA &&
                    hand[b].rank == hand.GetStrightRank(b + 1) + 2)
                    connectorCount++;

                return connectorCount == 2;
            }
        }

        var strightConnectCount = 0;
        // strights[*].count == 1
        for (var i = 0; i < strights.Count - 1; i++)
        {
            var (a, b) = (strights[i].i, strights[i + 1].i - 1);

            if (hand[a].rank == hand[b].rank + 2)
                strightConnectCount++;
        }

        return strightConnectCount >= 2;
    }

    public static bool HasFlush(this SortedHand hand)
    {
        var counts = new int[4];

        for (var i = 0; i < hand.Count; i++)
        {
            counts[hand[i].suit.ToSuitIndex()]++;
        }

        return counts[0] >= 5 || counts[1] >= 5 || counts[2] >= 5 || counts[3] >= 5;
    }

    public static bool HasFlushDrawStrict(this SortedHand hand) => hand.HasFlushDraw() && !hand.HasFlush();

    public static bool HasFlushDraw(this SortedHand hand)
    {
        var counts = new int[4];

        for (var i = 0; i < hand.Count; i++)
        {
            counts[hand[i].suit.ToSuitIndex()]++;
        }

        return counts[0] >= 4 || counts[1] >= 4 || counts[2] >= 4 || counts[3] >= 4;
    }

    public static bool HasHandPair(this Card[] hand) 
    { 
        if (hand.Length != 2) throw new ArgumentException("only hand of 2");
        
        return hand[0].rank == hand[1].rank;
    }

    public static bool HasFlushPair(this Card[] hand)
    {
        var counts = new int[4];

        for (var i = 0; i < hand.Length; i++)
        {
            counts[hand[i].suit.ToSuitIndex()]++;
        }

        return counts[0] >= 2 || counts[1] >= 2 || counts[2] >= 2 || counts[3] >= 2;
    }

    public static bool HasHandConnectors(this Card[] hand)
    {
        if (hand.Length != 2) throw new ArgumentException("only hand of 2");

        return Math.Abs(hand[0].rank - hand[1].rank) == 1 && hand[0].rank != Cards.RankA && hand[1].rank != Cards.RankA;
    }

    public static bool HasHandSuited(this Card[] hand)
    {
        if (hand.Length != 2) throw new ArgumentException("only hand of 2");

        return hand[0].suit == hand[1].suit;
    }

    public static bool HasHandSuitedConnectors(this Card[] hand) => hand.HasHandSuited() && hand.HasHandConnectors();

    public static bool HasFullHouse(this SortedHand hand)
    {
        var setRank = -1;

        for (var i = 0; i < hand.Count - 2; i++)
        {
            if (hand[i].rank == hand[i + 1].rank && hand[i].rank == hand[i + 2].rank)
                setRank = hand[i].rank;
        }

        if (setRank == -1)
            return false;

        int count = 0;

        for (var i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].rank == setRank)
                continue;

            if (hand[i].rank == hand[i + 1].rank)
                return true;
        }

        return false;
    }

    public static bool HasStraightFlush(this SortedHand hand)
    {
        var count = 0;
        var streetI = -1;

        for (var i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].rank == hand[i + 1].rank + 1)
                count++;
            else
                count = 0;

            if (count == 4)
            {
                streetI = i;
                break;
            }
        }

        if (hand.LastCard.rank == 2 && hand.FirstCard.rank == 14)
            count++;

        if (count == 4 && streetI == -1)
            streetI = 0;

        if (streetI > 0)
        {
            var s = '_';

            for (var i = 0; i < 5; i++)
            {
                var k = (Cards.cards.Length + streetI - i) % Cards.cards.Length;

                if (s == '_')
                    s = hand[k].suit;
                else if (hand[k].suit != s)
                    return false;
            }

            return true;
        }

        return false;
    }
}