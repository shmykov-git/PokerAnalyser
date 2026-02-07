using System;

namespace Model;

public class SortedHand
{
    public Card[] cards = [];
    public int Count => cards.Length;
    public Card this[int i] => cards[i];

    public Card FirstCard => cards[0];
    public Card LastCard => cards[^1];
    public int GetStrightRank(int i) => i == Count ? FirstCard.rank - 13 : cards[i].rank;

    public static implicit operator SortedHand((char c, char s)[] vs) => new SortedHand 
    { 
        cards = vs.Select(v => new Card(v)).OrderByDescending(c => c.rank).ThenBy(c => c.suit).ToArray() 
    };

    public override string ToString() => string.Join("  ", cards.Select(v => $"{v.rank.ToCardStr()}{v.suit}"));
}
