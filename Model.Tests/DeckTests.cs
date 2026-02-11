using FluentAssertions;

namespace Model.Tests;

[TestClass]
public sealed class DeckTests
{
    // '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
    // '♥', '♦', '♣', '♠'

    [TestMethod]
    public void TakeCardTest()
    {
        var deck = new Deck(new Random(0));
        SortedHand hand = new[] { deck.TakeCard(), deck.TakeCard(), deck.TakeCard(), deck.TakeCard(), deck.TakeCard(), deck.TakeCard(), deck.TakeCard() };
        SortedHand actualDeck = deck.cards.Concat(hand.cards).ToArray();

        Cards.deckCards.Should().BeEquivalentTo(actualDeck.cards);
    }

    [TestMethod]
    public void TakeSuitedCardTest()
    {
        var suit = '♦';

        var deck = new Deck(new Random(0));
        SortedHand hand = new[] { deck.TakeSuitedCard(suit), deck.TakeSuitedCard(suit), deck.TakeSuitedCard(suit), deck.TakeSuitedCard(suit), deck.TakeSuitedCard(suit) };
        SortedHand actualDeck = deck.cards.Concat(hand.cards).ToArray();

        hand.HasFlush().Should().BeTrue();
        Cards.deckCards.Should().BeEquivalentTo(actualDeck.cards);
    }

    [TestMethod]
    public void TakeExactCardTest()
    {
        var deck = new Deck(new Random(0));
        SortedHand hand = new[] { deck.TakeExactCard("J♦"), deck.TakeExactCard("J♠") };
        SortedHand actualDeck = deck.cards.Concat(hand.cards).ToArray();

        hand.HasPair().Should().BeTrue();
        Cards.deckCards.Should().BeEquivalentTo(actualDeck.cards);
    }
}
