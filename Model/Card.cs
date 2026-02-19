using System;

namespace Model;

public struct Card : IEquatable<Card>
{
    public int rank;
    public char suit;

    public Card(string card)
    {
        if (card == null || card.Length != 2) throw new ArgumentException($"Invalid card {card}");
        var (c, s) = (card[0], card[1]);

        if (!Cards.cards.Contains(c) || !Cards.suits.Contains(s)) throw new ArgumentException($"Invalid card {card}");
        rank = Cards.ranks[c];
        suit = s;
    }

    public Card(char c, char s)
    {
        rank = Cards.ranks[c];
        suit = s;
    }

    public Card((char c, char s) v)
    {
        rank = Cards.ranks[v.c];
        suit = v.s;
    }

    public static bool operator ==(Card a, Card b) => a.Equals(b);
    public static bool operator !=(Card a, Card b) => !a.Equals(b);

    public static implicit operator Card((char c, char s) v) => new Card(v);
    public static implicit operator Card(string v) => new Card(v);

    public bool Equals(Card other) => rank == other.rank && suit == other.suit;
    public override bool Equals(object obj) => obj is Card card && Equals(card);

    public override string ToString() => $"{rank.ToCardStr()}{suit}";
}
