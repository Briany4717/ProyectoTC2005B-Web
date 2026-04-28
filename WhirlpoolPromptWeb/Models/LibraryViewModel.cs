public class LibraryViewModel
{
    public List<Prompt> Prompts { get; set; }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public string SearchTerm { get; set; }
    public string SelectedCategory { get; set; }
    public string SortOrder { get; set; }
}
