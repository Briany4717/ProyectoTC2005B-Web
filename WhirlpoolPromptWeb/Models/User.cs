public class User
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public int Coins { get; set; }
    public ProfilePhoto ProfilePhoto { get; set; }
}

public enum ProfilePhoto
{
    mario,
    peach,
    bowser,
    luigi,
    yoshi
}