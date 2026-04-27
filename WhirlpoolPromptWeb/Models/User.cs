public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public int Coins { get; set; }
    public ProfilePhoto ProfilePhoto { get; set; }
    public int LocalRanking { get; set; }
    public int NationalRanking { get; set; }
}

public enum ProfilePhoto
{
    mario,
    peach,
    bowser,
    luigi,
    yoshi
}