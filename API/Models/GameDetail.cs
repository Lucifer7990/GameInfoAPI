public class GameDetail
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // public DateTime ReleaseDate { get; set; }
    // public string Developer { get; set; } = string.Empty;
    // public string Publisher { get; set; } = string.Empty;
    // public string Genre { get; set; } = string.Empty;
    // public string Platform { get; set; } = string.Empty;
    // public double Rating { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
}