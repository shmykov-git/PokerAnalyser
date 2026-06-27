namespace Model;

public class AnalyseTask 
{ 
    public GameCase GameCase { get; set; }
    public Card[]? MyHand { get; set; } = null;
    public Card[]? OponentHand { get; set; } = null;
    public Func<SortedHand[], bool> ConditionsFn { get; set; } = _ => true;
    public Func<SortedHand, Card[], Card[], bool> ConditionFn { get; set; } = (_, _, _) => true;
    public Func<SortedHand, Card[], Card[], Card[], bool> ConditionRFn { get; set; } = (_, _, _, _) => true;
    public Func<SortedHand, Card[], Card[], bool> MyConditionFn { get; set; } = (_, _, _) => true;
    public Func<SortedHand, Card[], Card[], Card[], bool> MyConditionRFn { get; set; } = (_, _, _, _) => true;
    public TableCardsCase Case { get; set; } = TableCardsCase.None;
    public int? IterationCount { get; set; } = 100_000;
    public (int from, int to) TableRange { get; set; } = (1, 1);
    public Func<SortedHand, bool> CombinationFn { get; set; } = _ => true;
    public Func<SortedHand[], bool> CombinationsFn { get; set; } = _ => true;
    public Func<SortedHand[], bool> OtherCombinationsFn { get; set; } = _ => true;
    public Func<SortedHand, bool> MyCombinationFn { get; set; } = _ => true;
    public string Description { get; set; }
}
