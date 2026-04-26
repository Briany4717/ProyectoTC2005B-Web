public class Prompt
{
    public int Id { get; set; }
    public string Title { get; set; }

    public Tag Tag { get; set; }

    public string Content { get; set; }

    public DateTime date { get; set; }

    public int[] Comments { get; set; }

    public int Likes { get; set; }
}

public class Tag
{
    public string Icon { get; set; }
    public string Label { get; set; }
}