namespace Model;

public class AnalyseTask 
{ 
    public GameCase Case { get; set; }
    public Func<Card[], SortedHand, bool>? CaseConditionFn { get; set; }
    public int IterationCount { get; set; }
    public (int from, int to) TableRange { get; set; }
    public Func<SortedHand, bool> CombinationFn { get; set; }
    public string Description { get; set; }
}
