using System;
using System.Reflection.Metadata.Ecma335;

namespace Model;

public class SortedHand
{
    public Card[] cards = [];
    public int Count => cards.Length;
    public Card this[int i] => cards[i];

    public Card FirstCard => cards[0];
    public Card LastCard => cards[^1];
    public int GetStrightRank(int i) => i == Count ? FirstCard.rank - 13 : cards[i].rank;

    public SortedHand(IEnumerable<Card> cards)
    {
        this.cards = cards.OrderByDescending(c => c.rank).ThenBy(c => c.suit).ToArray();
    }

    public SortedHand Join(Card[] flop) => cards.Concat(flop).ToArray();
    public SortedHand Join(Card[] flop, Card[] turn) => cards.Concat(flop).Concat(turn).ToArray();

    public static implicit operator SortedHand((char c, char s)[] vs) => new SortedHand(vs.Select(v => new Card(v)));
    public static implicit operator SortedHand(string[] vs) => new SortedHand(vs.Select(v => new Card(v)));
    public static implicit operator SortedHand(Card[] cards) => new SortedHand(cards);

    public override string ToString() => string.Join("  ", cards);
}
