namespace Model;

public struct Card
{
    public int rank;
    public char suit;

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

    public static implicit operator Card((char c, char s) v) => new Card(v);
}
