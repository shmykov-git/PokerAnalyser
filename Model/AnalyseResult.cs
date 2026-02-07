namespace Model;

public class AnalyseResult
{
    public string Description { get; set; }
    public List<Table> Tables { get; } = new();

    public class Table
    {
        public int N { get; set; }
        public double Probability { get; set; }
    }
}