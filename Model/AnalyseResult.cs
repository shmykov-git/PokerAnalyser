namespace Model;

public class AnalyseResult
{
    public string Description { get; set; }

    public List<Table> Tables { get; } = new();

    public Table? AverageTable { get; set; }

    public class Table
    {
        public decimal N { get; set; }
        public double Probability { get; set; }
        public string Explanation { get; set; }
    }
}