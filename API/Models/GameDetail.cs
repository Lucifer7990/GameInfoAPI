using System.ComponentModel.DataAnnotations;

public class GameDetail
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Url]
    [Required]
    [MinLength(1)]
    public string CoverImageUrl { get; set; } = string.Empty;
}

public class GameDetailDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [MinLength(1)]
    [Url]
    public string CoverImageUrl { get; set; } = string.Empty;
}