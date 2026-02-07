namespace Model;

public class Deck
{
    private HashSet<int> indices;
    private readonly Random rnd;

    private Card[] cards => Cards.deckCards;

    public Deck(Random rnd)
    {
        indices = cards.Select((_, i) => i).ToHashSet();
        this.rnd = rnd;
    }

    public Card TakeCard()
    {
        var k = rnd.Next(0, cards.Length);
        indices.Remove(k);
        return cards[k];
    }

    public SortedHand TakeHand()
    {
        return new[] { TakeCard(), TakeCard() };
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