using System.ComponentModel.DataAnnotations;

namespace Model;

public class Deck
{
    private readonly Random rnd;
    public List<Card> cards = Cards.deckCards.ToList();

    public Deck(Random rnd)
    {
        this.rnd = rnd;
    }

    public Card TakeExactCard(Card card)
    {
        var k = cards.IndexOf(card);
        cards.RemoveAt(k);
        return card;
    }

    public Card TakeCard()
    {
        var k = rnd.Next(0, cards.Count);
        var card = cards[k];
        cards.RemoveAt(k);
        return card;
    }

    public Card TakeSuitedCard(char suit)
    {
        var suitedIndices = cards.Select((c, i) => (c, i)).Where(v => v.c.suit == suit).Select(v => v.i).ToArray();
        if (suitedIndices.Length == 0)      
            throw new InvalidOperationException("No more cards of the same suit available");

        var i = rnd.Next(0, suitedIndices.Length);
        var k = suitedIndices[i];
        var card = cards[k];
        cards.RemoveAt(k);
        return card;
    }

    public SortedHand TakeHand()
    {
        return new[] { TakeCard(), TakeCard() };
    }

    public SortedHand TakeSuitedHand(char suit)
    {
        return new[] { TakeSuitedCard(suit), TakeSuitedCard(suit) };
    }

    public SortedHand TakeExactHand(Card[] hand)
    {
        hand.ThrowIfNotHand();
        return new[] { TakeExactCard(hand[0]), TakeExactCard(hand[1]) };
    }

    public Card[] TakeTableCards(GameCase gameCase) => gameCase switch
    {
        GameCase.PreFlop => [],
        GameCase.Flop => [TakeCard(), TakeCard(), TakeCard()],
        GameCase.Turn => [TakeCard(), TakeCard(), TakeCard(), TakeCard()],
        GameCase.River => [TakeCard(), TakeCard(), TakeCard(), TakeCard(), TakeCard()],
        _ => throw new NotImplementedException(gameCase.ToString())
    };
}