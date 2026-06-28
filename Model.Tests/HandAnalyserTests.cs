namespace Model.Tests;

[TestClass]
public sealed class HandAnalyserTests
{
    // '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    // '♥', '♦', '♣', '♠'

    [TestMethod]
    public void WinPairTest()
    {
        SortedHand h1 = new[] { "T♠", "K♠", "6♦", "5♥", "K♦", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.Pair);

        SortedHand h2 = new[] { "T♠", "K♠", "5♦", "5♥", "Q♦", "2♣", "3♣" };
        Card[] wh2 = h2.GetWinHand(Combination.Pair);
    }

    [TestMethod]
    public void WinTwoPairsTest()
    {
        SortedHand h1 = new[] { "T♠", "K♠", "5♦", "5♥", "K♦", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.TwoPairs);

        SortedHand h2 = new[] { "T♠", "K♠", "5♦", "5♥", "Q♦", "2♣", "T♣" };
        Card[] wh2 = h2.GetWinHand(Combination.TwoPairs);
    }

    [TestMethod]
    public void WinSetTest()
    {
        SortedHand h1 = new[] { "T♠", "K♠", "4♦", "K♥", "K♦", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.Set);

        SortedHand h2 = new[] { "T♠", "K♠", "5♦", "5♥", "Q♦", "2♣", "5♣" };
        Card[] wh2 = h2.GetWinHand(Combination.Set);
    }

    [TestMethod]
    public void WinStrightTest()
    {
        SortedHand h1 = new[] { "A♠", "K♠", "4♦", "K♥", "5♦", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.Stright);

        SortedHand h2 = new[] { "T♠", "K♠", "8♦", "7♥", "6♦", "4♣", "5♣" };
        Card[] wh2 = h2.GetWinHand(Combination.Stright);

        SortedHand h3 = new[] { "A♠", "K♠", "4♦", "6♥", "5♦", "2♣", "3♣" };
        Card[] wh3 = h3.GetWinHand(Combination.Stright);

        SortedHand h4 = new[] { "A♠", "7♠", "4♦", "6♥", "5♦", "2♣", "3♣" };
        Card[] wh4 = h4.GetWinHand(Combination.Stright);
    }

    [TestMethod]
    public void WinFlushTest()
    {
        SortedHand h1 = new[] { "9♥", "K♠", "4♦", "K♥", "6♥", "2♥", "5♥" };
        Card[] wh1 = h1.GetWinHand(Combination.Flush);

        SortedHand h2 = new[] { "5♥", "A♥", "4♥", "K♥", "6♥", "2♥", "9♥" };
        Card[] wh2 = h2.GetWinHand(Combination.Flush);
    }

    [TestMethod]
    public void WinFullHouseTest()
    {
        SortedHand h1 = new[] { "5♠", "K♠", "4♦", "K♥", "5♦", "2♣", "5♣" };
        Card[] wh1 = h1.GetWinHand(Combination.FullHouse);

        SortedHand h2 = new[] { "5♠", "K♠", "J♦", "K♥", "K♦", "5♣", "5♣" };
        Card[] wh2 = h2.GetWinHand(Combination.FullHouse);
    }

    [TestMethod]
    public void WinFourOfAKindTest()
    {
        SortedHand h1 = new[] { "T♠", "K♠", "K♣", "K♥", "K♦", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.FourOfAKind);

        SortedHand h2 = new[] { "5♠", "K♠", "5♦", "5♥", "Q♦", "2♣", "5♣" };
        Card[] wh2 = h2.GetWinHand(Combination.FourOfAKind);
    }

    [TestMethod]
    public void WinStrightFlushTest()
    {
        SortedHand h1 = new[] { "A♣", "K♠", "4♣", "K♥", "5♣", "2♣", "3♣" };
        Card[] wh1 = h1.GetWinHand(Combination.StrightFlush);

        SortedHand h2 = new[] { "T♠", "K♠", "8♣", "7♣", "6♣", "4♣", "5♣" };
        Card[] wh2 = h2.GetWinHand(Combination.StrightFlush);

        SortedHand h3 = new[] { "A♣", "K♠", "4♣", "6♣", "5♣", "2♣", "3♣" };
        Card[] wh3 = h3.GetWinHand(Combination.StrightFlush);

        SortedHand h4 = new[] { "A♣", "7♣", "4♣", "6♣", "5♣", "2♣", "3♣" };
        Card[] wh4 = h4.GetWinHand(Combination.StrightFlush);
    }
}
