public class ProfileViewModel
{
    public User User { get; set; }
    public List<Prompt> Prompts { get; set; }

    // Datos para la paginación
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    // Para mantener el estado en la vista
    public string SearchTerm { get; set; }
    public string ActiveTab { get; set; }
    public string SortOrder { get; set; }
}