using FluentAssertions;

namespace Model.Tests;

[TestClass]
public sealed class HandCheckerTests
{
    // '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    // '♥', '♦', '♣', '♠'

    [TestMethod]
    public void HasPairTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('5', '♥'), ('K', '♦') };
        h1.HasPair('A'.ToRank()).Should().BeFalse();
        h1.HasPair('K'.ToRank()).Should().BeTrue();

        SortedHand h2 = new[] { ('K', '♠'), ('K', '♦'), ('K', '♣'), ('6', '♦'), ('5', '♥') };
        h2.HasPair('A'.ToRank()).Should().BeFalse();
        h2.HasPair('K'.ToRank()).Should().BeTrue();
    }

    [TestMethod]
    public void HasTowPairTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasTwoPairs('A'.ToRank()).Should().BeFalse();
        h1.HasTwoPairs('K'.ToRank()).Should().BeTrue();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♦'), ('K', '♦'), ('K', '♣'), ('5', '♦'), ('5', '♥') };
        h2.HasTwoPairs('A'.ToRank()).Should().BeFalse();
        h2.HasTwoPairs('K'.ToRank()).Should().BeTrue();

        SortedHand h3 = new[] { ('K', '♠'), ('4', '♦'), ('K', '♦'), ('K', '♣'), ('5', '♦'), ('3', '♥') };
        h3.HasTwoPairs('K'.ToRank()).Should().BeFalse();
    }

    [TestMethod]
    public void HasSetTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasSet().Should().BeFalse();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♦'), ('K', '♦'), ('K', '♣'), ('5', '♦'), ('5', '♥') };
        h2.HasSet().Should().BeTrue();
    }

    [TestMethod]
    public void HasFourOfAKindTest()
    {
        SortedHand h1 = new[] { ('6', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('6', '♣'), ('4', '♦') };
        h1.HasFourOfAKind().Should().BeTrue();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♦'), ('K', '♦'), ('K', '♣'), ('5', '♦'), ('5', '♥') };
        h2.HasFourOfAKind().Should().BeFalse();
    }

    [TestMethod]
    public void HasFlushTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasFlush().Should().BeFalse();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♠'), ('Q', '♠'), ('K', '♠'), ('5', '♦'), ('5', '♠') };
        h2.HasFlush().Should().BeTrue();

        SortedHand h3 = new[] { ('K', '♠'), ('4', '♠'), ('Q', '♠'), ('K', '♠'), ('5', '♠'), ('6', '♠') };
        h3.HasFlush().Should().BeTrue();
    }

    [TestMethod]
    public void HasFlushDrawTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♠'), ('K', '♦'), ('4', '♦') };
        h1.HasFlushDraw().Should().BeFalse();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♠'), ('Q', '♠'), ('K', '♦'), ('5', '♦'), ('5', '♠') };
        h2.HasFlushDraw().Should().BeTrue();
    }

    [TestMethod]
    public void HasStraightTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasStraight().Should().BeFalse();

        SortedHand h2 = new[] { ('8', '♠'), ('4', '♦'), ('K', '♦'), ('7', '♣'), ('5', '♦'), ('6', '♥') };
        h2.HasStraight().Should().BeTrue();

        SortedHand h3 = new[] { ('A', '♠'), ('J', '♦'), ('K', '♦'), ('7', '♣'), ('T', '♦'), ('Q', '♥') };
        h3.HasStraight().Should().BeTrue();

        SortedHand h4 = new[] { ('A', '♠'), ('5', '♦'), ('3', '♦'), ('4', '♣'), ('T', '♦'), ('2', '♥') };
        h4.HasStraight().Should().BeTrue();

        SortedHand h5 = new[] { ('A', '♠'), ('5', '♦'), ('3', '♦'), ('4', '♣'), ('6', '♦'), ('2', '♥') };
        h5.HasStraight().Should().BeTrue();

        SortedHand h6 = new[] { ('A', '♠'), ('5', '♦'), ('3', '♦'), ('7', '♣'), ('6', '♦'), ('2', '♥') };
        h6.HasStraight().Should().BeFalse();
    }

    [TestMethod]
    public void HasStraightDrawTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasStraightDraw().Should().BeFalse();

        SortedHand h2 = new[] { ('9', '♠'), ('4', '♦'), ('K', '♦'), ('7', '♣'), ('5', '♦'), ('6', '♥') };
        h2.HasStraightDraw().Should().BeTrue();

        SortedHand h3 = new[] { ('A', '♠'), ('J', '♦'), ('K', '♦'), ('7', '♣'), ('9', '♦'), ('Q', '♥') };
        h3.HasStraightDraw().Should().BeTrue();

        SortedHand h4 = new[] { ('A', '♠'), ('7', '♦'), ('3', '♦'), ('4', '♣'), ('T', '♦'), ('2', '♥') };
        h4.HasStraightDraw().Should().BeTrue();

        SortedHand h5 = new[] { ('9', '♠'), ('5', '♦'), ('3', '♦'), ('4', '♣'), ('6', '♦'), ('2', '♥') };
        h5.HasStraightDraw().Should().BeTrue();

        SortedHand h6 = new[] { ('A', '♠'), ('5', '♦'), ('3', '♦'), ('7', '♣'), ('6', '♦'), ('2', '♥') };
        h6.HasStraightDraw().Should().BeTrue();

        SortedHand h7 = new[] { ('A', '♠'), ('5', '♦'), ('3', '♦'), ('7', '♣'), ('8', '♦'), ('2', '♥') };
        h7.HasStraightDraw().Should().BeTrue();

        SortedHand h8 = new[] { ('A', '♠'), ('5', '♦'), ('9', '♦'), ('7', '♣'), ('8', '♦'), ('J', '♥') };
        h8.HasStraightDraw().Should().BeTrue();

        SortedHand h9 = new[] { ('A', '♠'), ('5', '♦'), ('6', '♦'), ('9', '♣'), ('8', '♦'), ('J', '♥') };
        h9.HasStraightDraw().Should().BeTrue();
    }

    [TestMethod]
    public void HasDoubleStraightDrawTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasDoubleStraightDraw().Should().BeFalse();

        SortedHand h2 = new[] { ('9', '♠'), ('4', '♦'), ('K', '♦'), ('7', '♣'), ('5', '♦'), ('6', '♥') };
        h2.HasDoubleStraightDraw().Should().BeTrue();

        SortedHand h3 = new[] { ('A', '♠'), ('J', '♦'), ('K', '♦'), ('7', '♣'), ('9', '♦'), ('Q', '♥') };
        h3.HasDoubleStraightDraw().Should().BeFalse();

        SortedHand h4 = new[] { ('A', '♠'), ('7', '♦'), ('3', '♦'), ('4', '♣'), ('T', '♦'), ('2', '♥') };
        h4.HasDoubleStraightDraw().Should().BeFalse();

        SortedHand h5 = new[] { ('9', '♠'), ('5', '♦'), ('3', '♦'), ('4', '♣'), ('6', '♦'), ('2', '♥') };
        h5.HasDoubleStraightDraw().Should().BeTrue();

        SortedHand h6 = new[] { ('9', '♠'), ('5', '♦'), ('3', '♦'), ('8', '♣'), ('6', '♦'), ('2', '♥') };
        h6.HasDoubleStraightDraw().Should().BeTrue();

        SortedHand h7 = new[] { ('A', '♠'), ('K', '♦'), ('J', '♦'), ('T', '♣'), ('8', '♦'), ('7', '♥') };
        h7.HasDoubleStraightDraw().Should().BeTrue();

        SortedHand h8 = new[] { ('A', '♠'), ('5', '♦'), ('9', '♦'), ('7', '♣'), ('8', '♦'), ('J', '♥') };
        h8.HasDoubleStraightDraw().Should().BeTrue();

        SortedHand h9 = new[] { ('A', '♠'), ('5', '♦'), ('6', '♦'), ('9', '♣'), ('8', '♦'), ('J', '♥') };
        h9.HasDoubleStraightDraw().Should().BeFalse();
    }

    [TestMethod]
    public void HasFullHouseTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasFullHouse().Should().BeFalse();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♠'), ('K', '♦'), ('K', '♣'), ('5', '♦'), ('5', '♠') };
        h2.HasFullHouse().Should().BeTrue();

        SortedHand h3 = new[] { ('K', '♠'), ('5', '♠'), ('Q', '♠'), ('K', '♣'), ('5', '♠'), ('5', '♣') };
        h3.HasFullHouse().Should().BeTrue();
    }

    [TestMethod]
    public void HasStraightFlushTest()
    {
        SortedHand h1 = new[] { ('T', '♠'), ('K', '♠'), ('6', '♦'), ('6', '♥'), ('K', '♦'), ('4', '♦') };
        h1.HasStraightFlush().Should().BeFalse();

        SortedHand h2 = new[] { ('K', '♠'), ('4', '♠'), ('8', '♠'), ('7', '♠'), ('5', '♠'), ('6', '♠') };
        h2.HasStraightFlush().Should().BeTrue();
    }
}
