namespace Model;

public static class Cards
{
    public static char[] cards = [ '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' ];
    public static char[] suits = ['♠', '♣', '♥', '♦'];

    public static Dictionary<char, int> ranks = cards.ToDictionary(c => c, c => Array.IndexOf(cards, c) + 2);
    public static Card[] deckCards = suits.SelectMany(s => cards.Select(c => new Card(c, s))).ToArray();

    public const int RankJ = 11;
    public const int RankQ = 12;
    public const int RankK = 13;
    public const int RankA = 14;
    public const int SmallRankA = 1;

    public static int ToSuitIndex(this char s) => Array.IndexOf(suits, s);
    public static int ToRank(this char c) => ranks[c];
    public static char ToCard(this int r) => cards[r - 2];
    public static string ToCardStr(this int r) => r.ToCard().ToString().Replace("T", "10");
}
